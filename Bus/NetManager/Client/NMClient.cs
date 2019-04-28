using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Threading;

namespace NetManager.Client
{
    /// <summary>
    /// Класс для подключения к серверу и передачи параметров
    /// </summary>
    public class NMClient
    {
        private delegate void Action();
        private delegate void Action<T1, T2>(T1 a, T2 b);

        /// <summary>
        /// Владелец клиента. Для синхронизации потоков при вызове событий.
        /// Если синхронизация не требуется, то задать null
        /// </summary>
        public Control Owner
        {
            get;
            set;
        }

        /// <summary>
        /// Здесь хранится номер порта
        /// </summary>
        private int m_Port;

        /// <summary>
        /// Номер порта сервера, к которому идет подключение
        /// </summary>
        public int Port
        {
            get
            {
                return m_Port;
            }

            set
            {
                if (Port == value)
                    return;

                if (!IsRunning)
                {
                    if (value <= 0)
                        throw new ArgumentException();

                    m_Port = value;

                    PortChanged?.Invoke(this, new EventArgs());
                }
                else
                    throw new Exception("Нельзя изменить номер порта у запущенного клиента");
            }
        }

        /// <summary>
        /// Событие, возникающее когда изменен номер порта
        /// </summary>
        public event EventHandler PortChanged;

        /// <summary>
        /// Здесь хранится IP адрес сервера
        /// </summary>
        private IPAddress m_IPServer;

        /// <summary>
        /// Событие, возникающее когда IP сервера будет изменен
        /// </summary>
        public event EventHandler IPServerChanged;

        /// <summary>
        /// IP адрес сервера
        /// </summary>
        public IPAddress IPServer
        {
            get
            {
                return m_IPServer;
            }
            set
            {
                if (IPServer == value)
                    return;

                if (!IsRunning)
                {
                    m_IPServer = value;

                    IPServerChanged?.Invoke(this, new EventArgs());
                }
                else
                    throw new Exception("Нельзя изменить IP адрес сервера у запущенного клиента");
            }
        }

        /// <summary>
        /// Здесь хранится имя клиента
        /// </summary>
        private string m_Name;

        /// <summary>
        /// Название клиента, которое увидят все другие клиенты
        /// </summary>
        public string Name
        {
            get
            {
                return m_Name;
            }
            set
            {
                if (Name == value)
                    return;

                if (!IsRunning)
                {
                    if (value.Trim() == "")
                        throw new ArgumentException();

                    m_Name = value;

                    NameChanged?.Invoke(this, new EventArgs());
                }
                else
                    throw new Exception("Нельзя изменить название у запущенного клиента");
            }
        }

        /// <summary>
        /// Событие, возникающее когда название клиента изменено
        /// </summary>
        public event EventHandler NameChanged;

        /// <summary>
        /// Сообщает о том, подключен ли клиент к серверу
        /// </summary>
        public bool IsRunning
        {
            get;
            private set;
        }

        /// <summary>
        /// Событие, возникающее при ошибках
        /// </summary>
        public event EventHandler<EventMsgArgs> Error;

        /// <summary>
        /// Обработка ошибки - процедура синхронизируется с потоком, если Owner задан и вызывается событие
        /// </summary>
        /// <param name="msg">Сообщение об ошибке</param>
        private void ErrorEvent(string msg)
        {
            if (Owner != null && Owner.InvokeRequired)
            {
                Owner.Invoke(new Action<string>(ErrorEvent), msg);
            }
            else
            {
                Error?.Invoke(this, new EventMsgArgs(msg));
            }
        }

        /// <summary>
        /// Событие возникающее когда к серверу добавляется новый клиент
        /// </summary>
        public event EventHandler<EventClientArgs> NewClient;

        /// <summary>
        /// Обработка добавления нового клиента - процедура синхронизируется с потоком Control и вызывается событие
        /// </summary>
        /// <param name="id">Адрес клиента</param>
        /// <param name="name">Имя клиента</param>
        private void NewClientEvent(int id, string name)
        {
            if (Owner != null && Owner.InvokeRequired)
            {
                Owner.Invoke(new Action<int, string>(NewClientEvent), id, name);
            }
            else
                NewClient?.Invoke(this, new EventClientArgs(id, name));
        }

        /// <summary>
        /// Событие, возникающее когда от сервера отключается клиент
        /// </summary>
        public event EventHandler<EventClientArgs> DeleteClient;

        /// <summary>
        /// Обработка удаления нового клиента - процедура синхронизируется с потоком Control и вызывается событие
        /// </summary>
        /// <param name="id">Адрес клиента</param>
        /// <param name="name">Имя клиента</param>
        private void DeleteClientEvent(int id, string name)
        {
            if (Owner != null && Owner.InvokeRequired)
            {
                Owner.Invoke(new Action<int, string>(DeleteClientEvent), id, name);
            }
            else
                DeleteClient?.Invoke(this, new EventClientArgs(id, name));
        }

        /// <summary>
        /// Закрывается соединение с сервером
        /// </summary>
        /// <param name="client">Tcp клиент, по которому было осуществлено подключение</param>
        private void CloseConnect(TcpClient client)
        {
            try
            {
                byte[] buf = new byte[2 * sizeof(int)];
                byte[] tmp = BitConverter.GetBytes(0);
                for (int i = 0; i < tmp.Length; i++)
                    buf[i] = tmp[i];
                tmp = BitConverter.GetBytes(-1);
                for (int i = 0; i < tmp.Length; i++)
                    buf[sizeof(int) + i] = tmp[i];
                NetworkStream ns = client.GetStream();
                ns.Write(buf, 0, buf.Length);
            }
            catch { };
            client.Close();
            StopEvent();
        }

        /// <summary>
        /// Событие возникающее когда клиент останавливается
        /// </summary>
        public event EventHandler Stoped;

        /// <summary>
        /// Обработка отключения клиента - процедура синхронизируется с потоком Control и вызывается событие
        /// </summary>
        private void StopEvent()
        {
            if (Owner != null && Owner.InvokeRequired)
            {
                Owner.Invoke(new Action(StopEvent));
            }
            else
                Stoped?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Здес хранится список подключенных клиентов
        /// </summary>
        private List<ClientAddress> m_ClientAddresses;

        /// <summary>
        /// Список клиентов, подключенных к серверу
        /// </summary>
        public IList<ClientAddress> ClientAddresses
        {
            get
            {
                return m_ClientAddresses.AsReadOnly();
            }
        }

        /// <summary>
        /// Инструмент подключения к серверу по протоколу Tcp
        /// </summary>
        private TcpClient m_Client;

        /// <summary>
        /// Семафор для синхронизации получения данных
        /// </summary>
        private Semaphore m_SReseive;

        /// <summary>
        /// Событие, возникающее когда от сервера пришли данные
        /// </summary>
        public event EventHandler<EventClientMsgArgs> Reseive;

        /// <summary>
        /// Обработка получения данных - процедура синхронизируется с потоком Control и вызывается событие
        /// </summary>
        /// <param name="id">Адрес источника</param>
        /// <param name="msg">Массив байт данных полученных от источника</param>
        private void ReseiveEvent(int id, byte[] msg)
        {
            if ((Owner != null) && Owner.InvokeRequired)
            {
                Owner.Invoke(new Action<int, byte[]>(ReseiveEvent), id, msg);
            }
            else
            {
                m_SReseive.WaitOne();
                try
                {
                    int i = m_ClientAddresses.Count - 1;
                    while ((i >= 0) && (m_ClientAddresses[i].Id != id))
                        i--;
                    if (i >= 0)
                    {
                        string name = m_ClientAddresses[i].Name;
                        Reseive?.Invoke(this, new EventClientMsgArgs(id, name, msg));
                    }
                }
                finally
                {
                    m_SReseive.Release();
                }
            }
        }

        /// <summary>
        /// Считывает данные по сети
        /// </summary>
        /// <param name="ns">Соединение по сети</param>
        /// <param name="size">Размер считываемых данных</param>
        /// <returns></returns>
        private byte[] Read(NetworkStream ns, int size)
        {
            byte[] buf = new byte[size];
            int n = ns.Read(buf, 0, size);
            int s = 0;
            while (n != size)
            {
                s += n;
                size -= n;
                n = ns.Read(buf, s, size);
            }
            return buf;
        }

        /// <summary>
        /// Работа клиента (получение данных и команд от сервера)
        /// </summary>
        private void RunClient()
        {
            try
            {
                m_Client = new TcpClient(IPServer.AddressFamily);
                m_Client.SendBufferSize = 8388608;
                m_Client.ReceiveBufferSize = 8388608;
                m_Client.Connect(new IPEndPoint(IPServer, Port));
                NetworkStream ns = m_Client.GetStream();
                ns.Write(BitConverter.GetBytes(Name.Length), 0, sizeof(int));
                //передается на сервер имя клиента
                for (int i = 0; i < Name.Length; i++)
                {
                    byte[] buf = BitConverter.GetBytes(Name[i]);
                    ns.Write(buf, 0, buf.Length);
                }
                while (IsRunning && m_Client.Connected)
                {
                    int n = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);
                    if (IsRunning && m_Client.Connected)
                    {
                        if (n == 0)//обработка сообщений от сервера
                        {
                            n = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);
                            if (IsRunning && m_Client.Connected)
                            {
                                if (n != 0)//код команды
                                {
                                    if (n != 0x7FFFFFFF)
                                    {
                                        int id = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);
                                        if (IsRunning && m_Client.Connected)
                                        {
                                            int len = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);
                                            if (IsRunning && m_Client.Connected)
                                            {
                                                byte[] buf = Read(ns, len * sizeof(char));
                                                if (IsRunning && m_Client.Connected)
                                                {
                                                    string s = "";
                                                    for (int i = 0; i < buf.Length; i += sizeof(char))
                                                        s += BitConverter.ToChar(buf, i);
                                                    switch (n)
                                                    {
                                                        case 1:
                                                            m_ClientAddresses.Add(new ClientAddress(id, s));
                                                            NewClientEvent(id, s);
                                                            break;
                                                        case -1:
                                                            int i = m_ClientAddresses.Count - 1;
                                                            while ((i >= 0) && (m_ClientAddresses[i].Id != id))
                                                                i--;
                                                            if (i >= 0)
                                                            {
                                                                m_ClientAddresses.RemoveAt(i);
                                                                DeleteClientEvent(id, s);
                                                            }
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else //отключение
                                {
                                    m_ClientAddresses.Clear();
                                    IsRunning = false;
                                    m_Client.Close();
                                    StopEvent();
                                    return;
                                }
                            }
                        }
                        else //получение сообщения
                        {
                            int len = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);
                            if (IsRunning && m_Client.Connected)
                            {
                                byte[] buf = Read(ns, len);
                                if (IsRunning && m_Client.Connected)
                                    ReseiveEvent(n, buf);
                            }
                        }
                    }
                }
                CloseConnect(m_Client);
            }
            catch (Exception e)
            {
                if (IsRunning)
                {
                    IsRunning = false;
                    if (m_Client != null)
                        CloseConnect(m_Client);

                    ErrorEvent(e.Message);
                }
            }
        }

        /// <summary>
        /// Отправляет данные через сервер клиенту по указанному адресу
        /// <param name="address">Адрес назначения</param>
        /// <param name="data">Массив байт данных</param>
        /// </summary>
        public void SendData(int address, byte[] data)
        {
            int[] arr = new int[1];
            arr[0] = address;
            SendData(arr, data);
        }

        /// <summary>
        /// Отправляет данные через сервер клиентам по указанному адресу
        /// </summary>
        /// <param name="addresses">Массив адресов назначения</param>
        /// <param name="data">Массив байт данных</param>
        public void SendData(int[] addresses, byte[] data)
        {
            if (IsRunning && m_Client.Connected && (addresses.Length > 0) && (data.Length > 0))
            {
                NetworkStream ns = m_Client.GetStream();
                if (IsRunning && m_Client.Connected)
                {
                    byte[] tmp = new byte[8 + addresses.Length * sizeof(int) + data.Length];
                    Array.Copy(BitConverter.GetBytes(addresses.Length), 0, tmp, 0, sizeof(int));
                    if (IsRunning && m_Client.Connected)
                    {
                        int i = 0;
                        while ((i < addresses.Length) && IsRunning && m_Client.Connected)
                            Array.Copy(BitConverter.GetBytes(addresses[i++]), 0, tmp, i * sizeof(int), sizeof(int));
                        Array.Copy(BitConverter.GetBytes(data.Length), 0, tmp, (addresses.Length + 1) * sizeof(int), sizeof(int));
                        Array.Copy(data, 0, tmp, (addresses.Length + 2) * sizeof(int), data.Length);
                        if (IsRunning && m_Client.Connected)
                            ns.Write(tmp, 0, tmp.Length);
                    }
                }
            }
        }

        /// <summary>
        /// Событие возникающее когда процесс был запущен
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// Подключение клиента к серверу
        /// </summary>
        public void StartClient()
        {
            if (!IsRunning)
            {
                try
                {
                    IsRunning = true;

                    Thread thr = new Thread(RunClient);
                    thr.IsBackground = true;
                    thr.Start();

                    Started?.Invoke(this, new EventArgs());
                }
                catch (Exception e)
                {
                    IsRunning = false;
                    throw e;
                }
            }
            else
                throw new Exception("Клиент уже запущен");
        }

        /// <summary>
        /// Отключение клиента от сервера
        /// </summary>
        public void StopClient()
        {
            if (IsRunning)
            {
                IsRunning = false;
                CloseConnect(m_Client);
            }
            else
                throw new Exception("Клиент уже остановлен");
        }

        /// <summary>
        /// Заставляет сервер перезапуститься
        /// </summary>
        public void SendRestartServer()
        {
            if (IsRunning)
            {
                byte[] data = new byte[8];
                NetworkStream ns = m_Client.GetStream();
                data[0] = 0x00;
                data[1] = 0x00;
                data[2] = 0x00;
                data[3] = 0x00;
                data[4] = 0xFF;
                data[5] = 0xFF;
                data[6] = 0xFF;
                data[7] = 0x7F;
                ns.Write(data, 0, 8);
            }
        }

        /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="owner">Окно, внутри которого осуществляется управление клиентом.
        /// Используется для того, чтобы синхронизировать асинхронные процессы с процессом окна.
        /// Если синхронизация не требуется, его можно задать null</param>
        public NMClient(Control owner)
        {
            IsRunning = false;
            m_ClientAddresses = new List<ClientAddress>();
            m_SReseive = new Semaphore(1, 1);

            Owner = owner;
        }

        /// <summary>
        /// Конструктор 
        /// </summary>
        /// <param name="serverIP">IP адрес сервера</param>
        /// <param name="port">Номер порта сервера</param>
        /// <param name="control">Окно, внутри которого осуществляется управление клиентом.
        /// Используется для того, чтобы синхронизировать асинхронные процессы с процессом окна.
        /// Если синхронизация не требуется, его можно задать null</param>
        public NMClient(IPAddress serverIP, int port, Control control)
        {
            IsRunning = false;
            m_ClientAddresses = new List<ClientAddress>();
            m_SReseive = new Semaphore(1, 1);

            IPServer = serverIP;
            Port = port;
            Owner = control;
        }

        /// <summary>
        /// деструктор, в котором останавливается клиент, если он не был остановлен
        /// </summary>
        ~NMClient()
        {
            if (IsRunning)
                StopClient();
        }
    }
}
