using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Windows.Forms;

namespace NetManager.Server
{
    /// <summary>
    /// Элемент управления сервером (в принципе, в нем есть все, что нужно для сервера)
    /// </summary>
    public partial class ServerDaemon : ServerControl
    {
        private string m_ClientCount = "Число клиентов {0}";

        /// <summary>
        /// Список адресов машины, где запущен сервер (для справки)
        /// </summary>
        [Browsable(false)]
        public IList<IPAddress> IPAddressList
        {
            get
            {
                return new ReadOnlyCollection<IPAddress>(Dns.GetHostEntry(Dns.GetHostName()).AddressList);
            }
        }

        private void Init()
        {
            InitializeComponent();

            //определяется IP
            string[] Lines = new string[IPAddressList.Count];
            for (int I = 0; I < IPAddressList.Count; I++)
                Lines[I] = IPAddressList[I].AddressFamily.ToString() + ": " + IPAddressList[I].ToString();
            tbIPAddress.Lines = Lines;
            //настраиваются события сервера
            Server.Error += Server_Error;
            Server.Started += Server_Started;
            Server.Stoped += Server_Stoped;
            Server.Restarted += Server_Restarted;
            Server.ClientAdded += Server_ChangeClient;
            Server.ClientDeleted += Server_ChangeClient;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ServerDaemon()
        {
            Init();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="server">Управляемый сервер</param>
        public ServerDaemon(NMServer server) : base(server)
        {
            Init();
        }

        /// <summary>
        /// Сервер
        /// </summary>
        [Browsable(false)]
        public override NMServer Server
        {
            get
            {
                return base.Server;
            }

            set
            {
                if (Server == value)
                    return;

                if (value == null)
                    throw new ArgumentNullException();

                if ((Server != null && Server.IsRunning) || value.IsRunning)
                    throw new Exception("Нельзя изменять свойство когда старый или новый сервер запущен");

                if (Server != null)
                {
                    Server.Error -= Server_Error;
                    Server.Started -= Server_Started;
                    Server.Stoped -= Server_Stoped;
                    Server.Restarted -= Server_Restarted;
                    Server.ClientAdded -= Server_ChangeClient;
                    Server.ClientDeleted -= Server_ChangeClient;
                }

                base.Server = value;

                Server.Error += Server_Error;
                Server.Started += Server_Started;
                Server.Stoped += Server_Stoped;
                Server.Restarted += Server_Restarted;
                Server.ClientAdded += Server_ChangeClient;
                Server.ClientDeleted += Server_ChangeClient;
            }
        }

        private void Server_Started(object sender, EventArgs e)
        {
            btnStart.Text = "Стоп";
        }

        private void Server_ChangeClient(object sender, EventClientArgs e)
        {
            lClients.Text = string.Format(m_ClientCount, Server.Clients.Count);
            tbClients.Clear();
            for (int i = 0; i < Server.Clients.Count; i++)
            {
                tbClients.Text += Server.Clients[i].ToString();
                if (i < Server.Clients.Count - 1)
                    tbClients.Text += "\r\n";
            }
        }

        private void Server_Restarted(object sender, EventArgs e)
        {
            tbClients.Clear();
            lClients.Text = string.Format(m_ClientCount, 0);
        }

        private void Server_Stoped(object sender, EventArgs e)
        {
            btnStart.Enabled = true;
            btnStart.Text = "Старт";
            lClients.Text = string.Format(m_ClientCount, 0);
            tbClients.Clear();
        }

        private void Server_Error(object sender, EventMsgArgs e)
        {
            MessageBox.Show(e.Msg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Server.IsRunning)
            {
                Server.StopServer();
                btnStart.Enabled = false;
            }
            else
            {
                Server.StartServer();
                nPort.Enabled = false;
                btnStart.Text = "Стоп";
            }
        }
    }
}
