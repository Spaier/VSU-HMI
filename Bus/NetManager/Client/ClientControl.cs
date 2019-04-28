using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace NetManager.Client
{
    /// <summary>
    /// Элемент управления клиентом с возможностью выбора клиентов, которым нужно отправлять данные
    /// </summary>
    public partial class ClientControl : ReseiveClientControl
    {
        private void Init()
        {
            InitializeComponent();

            Client.NewClient += Client_NewClient;
            Client.DeleteClient += Client_DeleteClient;
            Client.Stoped += Client_Stoped;
        }

        private void Client_Stoped(object sender, EventArgs e)
        {
            chClients.Items.Clear();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="client">Управляемый клиент</param>
        public ClientControl(NMClient client) : base(client)
        {
            Init();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ClientControl()
        {
            Init();
        }

        /// <summary>
        /// Клиент
        /// </summary>
        [Browsable(false)]
        public override NMClient Client
        {
            get
            {
                return base.Client;
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
                    Client.NewClient -= Client_NewClient;
                    Client.DeleteClient -= Client_DeleteClient;
                }

                base.Client = value;

                Client.NewClient += Client_NewClient;
                Client.DeleteClient += Client_DeleteClient;
            }
        }

        private void Client_DeleteClient(object sender, EventClientArgs e)
        {
            var cl = new ClientAddress(e.ClientId, e.Name);
            var i = chClients.Items.Count - 1;
            while (i >= 0 && cl.Id != (chClients.Items[i] as ClientAddress).Id)
                i--;
            if (i >= 0)
                chClients.Items.RemoveAt(i);
        }

        private void Client_NewClient(object sender, EventClientArgs e)
        {
            var cl = new ClientAddress(e.ClientId, e.Name);
            chClients.Items.Add(cl);
        }

        /// <summary>
        /// Возвращает список данных о выбранных клиентах
        /// </summary>
        [Browsable(false)]
        public IList<ClientAddress> SelectedClients
        {
            get
            {
                var clients = new List<ClientAddress>(chClients.SelectedItems.Count);
                foreach (ClientAddress value in chClients.SelectedItems)
                    clients.Add(value);
                return clients.AsReadOnly();
            }
        }

        /// <summary>
        /// Возвращает массив Id выбранных клиентов
        /// </summary>
        [Browsable(false)]
        public int[] SelectedClientAddresses
        {
            get
            {
                var clients = new int[chClients.SelectedItems.Count];
                for (var i = 0; i < chClients.SelectedItems.Count; i++)
                    clients[i] = (chClients.SelectedItems[i] as ClientAddress).Id;
                return clients;
            }
        }
    }
}
