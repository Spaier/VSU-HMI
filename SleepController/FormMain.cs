using System;
using System.Windows.Forms;
using NetManager.Client;
using System.Xml;
using SleepController.Messages;
using System.Linq;

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

        private void Client_Error(object sender, NetManager.EventMsgArgs e)
        {
            MessageBox.Show(e.Msg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Client_Reseive(object sender, NetManager.EventClientMsgArgs e)
        {
            var message = new SleepControllerMessage(e.Msg);
            tbReseive.Text += $"{(new ClientAddress(e.ClientId, e.Name))}: Frequency = { message.frequency }";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (myClientControl.Client.IsRunning && myClientControl.SelectedClientAddresses.Length > 0 && tbSend.Text != "")
            {
                ///Подготовка данных к отправке
                byte[] msg = new byte[tbSend.Text.Length * 2 + 4];
                Array.Copy(BitConverter.GetBytes(18), msg, 4);
                for (int i = 0; i < tbSend.Text.Length; i++)
                    Array.Copy(BitConverter.GetBytes(tbSend.Text[i]), 0, msg, 4 + 2 * i, 2);

                myClientControl.Client.SendData(myClientControl.SelectedClientAddresses, msg);
                tbReseive.Text += "Отправлено: " + tbSend.Text + "\r\n\r\n";
                tbSend.Text = "";
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            LoadState();
        }

        private void LoadState()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(InitFileName);
                XmlElement netManager = xmlDoc["Settings"]["NetManager"];
                myClientControl.LoadState(netManager);
            }
            catch { }
        }

        private string InitFileName = "Settings.xml";

        private void SaveState()
        {
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                xmlDoc.Load(InitFileName);
            }
            catch
            {
                xmlDoc.AppendChild(xmlDoc.CreateElement("Settings"));
            }

            XmlElement root = xmlDoc.DocumentElement;

            XmlElement netManager = root["NetManager"];
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
