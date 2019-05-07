using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using SleepController.Domain;
using SleepController.Messages;

namespace ClientExample
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            myClientControl.Client.Reseive += Client_Reseive;
            myClientControl.Client.Error += Client_Error;
        }

        private SuperDetector ClosedEyesDetector { get; set; } = new SuperDetector()
        {
            ClosedEyesMinThreshold = 30,
        };

        private SuperDetector ArmPowerDetector { get; set; } = new SuperDetector()
        {
            ClosedEyesMinThreshold = 50,
        };

        private void Client_Error(object sender, NetManager.EventMsgArgs e)
        {
            MessageBox.Show(e.Msg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Client_Reseive(object sender, NetManager.EventClientMsgArgs e)
        {
            var message = new SleepControllerMessage(e.Msg);

            var eegIndex = 3;
            var eegSignal = message.Data
                .Skip(eegIndex * SleepControllerMessage.ChannelLength)
                .Take(SleepControllerMessage.ChannelLength);
            eyesStatusGroupBox.BackColor = ClosedEyesDetector.IsClosed(eegSignal)
                ? Color.AliceBlue
                : Color.Red;

            var armIndex = 22;
            var armSignal = message.Data
                .Skip(armIndex * SleepControllerMessage.ChannelLength)
                .Take(SleepControllerMessage.ChannelLength);
            armStatusGroupBox.BackColor = ClosedEyesDetector.IsClosed(armSignal)
                ? Color.AliceBlue
                : Color.Red;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadState();
        }

        private void LoadState()
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(InitFileName);
                var netManager = xmlDoc["Settings"]["NetManager"];
                myClientControl.LoadState(netManager);
            }
            catch { }
        }

        private string InitFileName { get; set; } = "Settings.xml";

        private void SaveState()
        {
            var xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(InitFileName);
            }
            catch
            {
                xmlDoc.AppendChild(xmlDoc.CreateElement("Settings"));
            }

            var root = xmlDoc.DocumentElement;

            var netManager = root["NetManager"];
            if (netManager == null)
            {
                netManager = xmlDoc.CreateElement("NetManager");
                root.AppendChild(netManager);
            }

            myClientControl.SaveState(netManager);

            xmlDoc.Save(InitFileName);
        }

        private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveState();
        }
    }
}
