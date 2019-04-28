using System.Windows.Forms;
using System.ComponentModel;
using System.Net;
using System;
using System.Xml;

namespace NetManager.Client
{
    /// <summary>
    /// Элемент управления с клиентом для задач, в которых не требуется отправка данных
    /// По умолчанию клиент работает в асинхронном режиме
    /// </summary>
    public partial class ReseiveClientControl : UserControl
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="client">Управляемый клиент</param>
        public ReseiveClientControl(NMClient client)
        {
            if (client == null)
                throw new ArgumentException();

            if (client.IsRunning)
                throw new Exception("Нельзя присваивать клиента элементу управления, когда он запущен");

            m_Client = client;

            InitializeComponent();

            tbIP.Text = m_Client.IPServer.ToString();
            tbPort.Text = m_Client.Port.ToString();
            tbName.Text = m_Client.Name;
            m_Client.IPServerChanged += Client_IpServerChange;
            m_Client.PortChanged += Client_PortChanged;
            m_Client.NameChanged += Client_NameChange;
            m_Client.Started += Client_Started;
            m_Client.Stoped += Client_Stoped;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ReseiveClientControl()
        {
            InitializeComponent();

            m_Client = new NMClient(null);
            m_Client.IPServer = IPAddress.Parse(tbIP.Text.Trim());
            m_Client.Port = int.Parse(tbPort.Text.Trim());
            m_Client.Name = tbName.Text;
            m_Client.IPServerChanged += Client_IpServerChange;
            m_Client.PortChanged += Client_PortChanged;
            m_Client.NameChanged += Client_NameChange;
            m_Client.Started += Client_Started;
            m_Client.Stoped += Client_Stoped;
        }

        private void Client_Started(object sender, EventArgs e)
        {
            tbIP.Enabled = false;
            tbPort.Enabled = false;
            tbName.Enabled = false;
        }

        private void Client_Stoped(object sender, EventArgs e)
        {
            tbIP.Enabled = true;
            tbPort.Enabled = true;
            tbName.Enabled = true;
        }

        private void Client_NameChange(object sender, System.EventArgs e)
        {
            tbName.Text = Client.Name;
        }

        private void Client_PortChanged(object sender, System.EventArgs e)
        {
            tbPort.Text = Client.Port.ToString();
        }

        private void Client_IpServerChange(object sender, System.EventArgs e)
        {
            tbIP.Text = Client.IPServer.ToString();
        }

        /// <summary>
        /// Клиент
        /// </summary>
        private NMClient m_Client;

        /// <summary>
        /// Клиент
        /// </summary>
        [Browsable(false)]
        public virtual NMClient Client
        {
            get
            {
                return m_Client;
            }
            set
            {
                if (Client == value)
                    return;

                if (value == null)
                    throw new ArgumentNullException();

                if ((Client != null && Client.IsRunning) || value.IsRunning)
                    throw new Exception("Нельзя изменить это свойство когда старый или новый клиент запущен");

                if (Client != null)
                {
                    Client.IPServerChanged -= Client_IpServerChange;
                    Client.PortChanged -= Client_PortChanged;
                    Client.NameChanged -= Client_NameChange;
                    Client.Started -= Client_Started;
                    Client.Stoped -= Client_Stoped;
                }

                Client = value;

                tbIP.Text = Client.IPServer.ToString();
                tbPort.Text = Client.Port.ToString();
                tbName.Text = Client.Name;
                Client.IPServerChanged += Client_IpServerChange;
                Client.PortChanged += Client_PortChanged;
                Client.NameChanged += Client_NameChange;
                Client.Started += Client_Started;
                Client.Stoped += Client_Stoped;
            }
        }

        /// <summary>
        /// Определяет, работает ли клиент в синхронном режиме
        /// </summary>
        [Category("NetManager"), Description("Определяет, работает ли клиент в синхронном режиме")]
        public bool IsSyncronized
        {
            get
            {
                return Client.Owner != null;
            }
            set
            {
                Client.Owner = value ? this : null;
            }
        }

        /// <summary>
        /// IP сервера
        /// </summary>
        [Category("NetManager"), Description("IP сервера"), TypeConverter(typeof(Design.IPAddressConverter))]
        public IPAddress IPServer
        {
            get
            {
                return Client.IPServer;
            }
            set
            {
                Client.IPServer = value;
            }
        }

        /// <summary>
        /// Номер порта сервера, к которому идет подключение
        /// </summary>
        [Category("NetManager"), Description("Номер порта сервера, к которому идет подключение")]
        public int Port
        {
            get
            {
                return Client.Port;
            }
            set
            {
                Client.Port = value;
            }
        }

        /// <summary>
        /// Название клиента, которое увидят все клиенты
        /// </summary>
        [Category("NetManager"), Description("Название клиента, которое увидят все клиенты")]
        public string ClientName
        {
            get
            {
                return Client.Name;
            }
            set
            {
                Client.Name = value;
            }
        }

        private void tbIP_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                Client.IPServer = IPAddress.Parse(tbIP.Text);
            }
            catch
            {
                tbIP.Text = Client.IPServer.ToString();
            }
        }

        private void tbPort_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                Client.Port = int.Parse(tbPort.Text);
            }
            catch
            {
                tbPort.Text = Client.Port.ToString();
            }
        }

        private void tbName_TextChanged(object sender, System.EventArgs e)
        {
            try
            {
                Client.Name = tbName.Text.Trim();
            }
            catch
            {
                tbName.Text = Client.Name;
            }
        }

        /// <summary>
        /// Сохраняются настройки компонента
        /// </summary>
        /// <param name="node">Узел с настройками</param>
        public void SaveState(XmlElement node)
        {
            node.SetAttribute("Name", ClientName);
            node.SetAttribute("Port", Port.ToString());
            node.SetAttribute("IP", IPServer.ToString());
        }

        /// <summary>
        /// Загружаются настройки компонента
        /// </summary>
        /// <param name="node">Узел с настройками</param>
        public void LoadState(XmlElement node)
        {
            ClientName = node.GetAttribute("Name");
            Port = int.Parse(node.GetAttribute("Port").Trim());
            IPServer = IPAddress.Parse(node.GetAttribute("IP").Trim());
        }

        /// <summary>
        /// Останавливается клиент, когда объект выгружается
        /// </summary>
        ~ReseiveClientControl()
        {
            if (Client.IsRunning)
                Client.StopClient();
        }
    }
}
