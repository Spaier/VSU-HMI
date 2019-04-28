using System;
using System.Windows.Forms;
using System.Xml;
using NetManager.Server;

namespace NMServer
{
    public partial class FormServer : Form
    {
        private const string InitFileName = "Settings.xml";

        public FormServer()
        {
            InitializeComponent();

            //попытка считать настройки программы
            LoadInit();
            serverDaemon.Server.Started += Server_Started;
            serverDaemon.Server.Stoped += Server_OnStop;
        }

        private void Server_Started(object sender, EventArgs e)
        {
            startToolStripMenuItem.Text = "Стоп";
        }

        void Server_OnStop(object sender, EventArgs e)
        {
            startToolStripMenuItem.Text = "Старт";
        }

        private void LoadInit()
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(InitFileName);
                serverDaemon.LoadState(xmlDoc["Settings"]["NetManager"]["Server"]["Network"]);
            }
            catch { }
        }

        private void SaveInit()
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

            var nm = root["NetManager"];
            if (nm == null)
            {
                nm = xmlDoc.CreateElement("NetManager");
                root.AppendChild(nm);
            }

            var el = nm["Server"];
            if (el == null)
            {
                el = xmlDoc.CreateElement("Server");
                nm.AppendChild(el);
            }

            nm = el["Network"];
            if (nm == null)
            {
                nm = xmlDoc.CreateElement("Network");
                el.AppendChild(nm);
            }

            serverDaemon.SaveState(nm);

            xmlDoc.Save(InitFileName);
        }

        private void FormServer_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serverDaemon.Server.IsRunning)
                serverDaemon.Server.StopServer();
            SaveInit();
        }

        private void FormServer_Resize(object sender, EventArgs e)
        {
            SetView();
        }

        public void SetView()
        {
            ShowInTaskbar = WindowState != FormWindowState.Minimized;
            if (!ShowInTaskbar)
            {
                Hide();
                showToolStripMenuItem.Text = "Показать";
            }
            else
            {
                Show();
                showToolStripMenuItem.Text = "Скрыть";
            }
        }

        private void ShowHide()
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
            else
                WindowState = FormWindowState.Minimized;
            SetView();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowHide();
        }

        private bool CanClose = false;

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CanClose = true;
            Close();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowHide();
        }

        private void FormServer_Load(object sender, EventArgs e)
        {
            serverDaemon.Server.StartServer();
        }

        private void FormServer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!CanClose)
            {
                e.Cancel = true;
                if (WindowState != FormWindowState.Minimized)
                    ShowHide();
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (serverDaemon.Server.IsRunning)
                serverDaemon.Server.StopServer();
            else
                serverDaemon.Server.StartServer();
        }
    }
}
