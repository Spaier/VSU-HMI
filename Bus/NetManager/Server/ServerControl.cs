using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml;

namespace NetManager.Server
{
    /// <summary>
    /// Пользовательский элемент управления для сервера
    /// </summary>
    public partial class ServerControl : UserControl
    {
        private NMServer m_Server;

        /// <summary>
        /// Сервер
        /// </summary>
        [Browsable(false)]
        public virtual NMServer Server
        {
            get
            {
                return m_Server;
            }
            set
            {
                if (Server == value)
                    return;

                if (value == null)
                    throw new ArgumentNullException();

                if ((Server != null && Server.IsRunning) || value.IsRunning)
                    throw new Exception("Нельзя изменять свойство когда старый или новый сервер запущен");

                if (m_Server != null)
                {
                    m_Server.PortChanged -= Server_PortChanged;
                    m_Server.LiveTimeChanged -= Server_LiveTimeChanged;
                    m_Server.Started -= Server_Started;
                    m_Server.Stoped -= Server_Stoped;
                }

                m_Server = value;

                nPort.Value = m_Server.Port;
                nLiveTime.Value = m_Server.LiveTime;
                m_Server.PortChanged += Server_PortChanged;
                m_Server.LiveTimeChanged += Server_LiveTimeChanged;
                m_Server.Started += Server_Started;
                m_Server.Stoped += Server_Stoped;
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="server">Управляемый сервер</param>
        public ServerControl(NMServer server)
        {
            if (server == null)
                throw new ArgumentNullException();

            if (server.IsRunning)
                throw new Exception("Нельзя присваивать сервер элементу управления, когда он запущен");

            m_Server = server;

            InitializeComponent();

            nPort.Value = m_Server.Port;
            nLiveTime.Value = m_Server.LiveTime;
            m_Server.PortChanged += Server_PortChanged;
            m_Server.LiveTimeChanged += Server_LiveTimeChanged;
            m_Server.Started += Server_Started;
            m_Server.Stoped += Server_Stoped;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ServerControl()
        {
            m_Server = new NMServer(this);

            InitializeComponent();

            m_Server.Port = (int)nPort.Value;
            m_Server.LiveTime = (int)nLiveTime.Value;
            m_Server.PortChanged += Server_PortChanged;
            m_Server.LiveTimeChanged += Server_LiveTimeChanged;
            m_Server.Started += Server_Started;
            m_Server.Stoped += Server_Stoped;
        }

        private void Server_LiveTimeChanged(object sender, EventArgs e)
        {
            nLiveTime.Value = Server.LiveTime;
        }

        private void Server_PortChanged(object sender, EventArgs e)
        {
            nPort.Value = Server.Port;
        }

        private void Server_Started(object sender, EventArgs e)
        {
            nPort.Enabled = false;
        }

        private void Server_Stoped(object sender, EventArgs e)
        {
            nPort.Enabled = true;
        }

        private void nPort_ValueChanged(object sender, EventArgs e)
        {
            Server.Port = (int)nPort.Value;
        }

        private void nTestLive_ValueChanged(object sender, EventArgs e)
        {
            Server.LiveTime = (int)nLiveTime.Value;
        }

        /// <summary>
        /// Номер порта
        /// </summary>
        [Category("NetManager"), Description("Номер порта")]
        public int Port
        {
            get
            {
                return (int)nPort.Value;
            }
            set
            {
                nPort.Value = value;
            }
        }

        /// <summary>
        /// Время, по истечении которого идет проверка, жив ли клиент.
        /// 0 - проверки не будет
        /// </summary>
        [Category("NetManager"), Description("Время, по истечении которого идет проверка, жив ли клиент. 0 - проверки не будет")]
        public int LiveTime
        {
            get
            {
                return (int)nLiveTime.Value;
            }
            set
            {
                nLiveTime.Value = value;
            }
        }

        /// <summary>
        /// Сохраняются настройки компонента
        /// </summary>
        /// <param name="node">Узел с настройками</param>
        public void SaveState(XmlElement node)
        {
            node.SetAttribute("Port", Port.ToString());
            node.SetAttribute("LiveTime", LiveTime.ToString());
        }

        /// <summary>
        /// Загружаются настройки компонента
        /// </summary>
        /// <param name="node">Узел с настройками</param>
        public void LoadState(XmlElement node)
        {
            Port = int.Parse(node.GetAttribute("Port").Trim());
            LiveTime = int.Parse(node.GetAttribute("LiveTime").Trim());
        }

        /// <summary>
        /// Останавливается сервер, когда элемент выгружается
        /// </summary>
        ~ServerControl()
        {
            if (Server.IsRunning)
                Server.StopServer();
        }
    }
}
