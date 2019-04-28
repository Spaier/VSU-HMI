using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Collections;

namespace NetManager.Server
{
    /// <summary>
    /// класс сервер
    /// </summary>
    public class NMServer
    {
        private delegate void Action();
        private delegate void Action<T1, T2>(T1 a, T2 b);

        /// <summary>
        /// используется для синхронизации потоков при передаче событий. если null, то синхронизация не происходит
        /// </summary>
        public Control Owner
        {
            get;
            set;
        }

        /// <summary>
        /// номер порта
        /// </summary>
        private int m_Port;

        /// <summary>
        /// true - запущено
        /// </summary>
        public bool IsRunning
        {
            get;
            private set;
        }

        /// <summary>
        /// номер порта
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
                    m_Port = value;

                    PortChanged?.Invoke(this, new EventArgs());
                }
                else
                    throw new Exception("Нельзя присвоить номер порта не остановив сервер");
            }
        }

        /// <summary>
        /// Событие, возникающее когда порт был изменен
        /// </summary>
        public event EventHandler PortChanged;

        /// <summary>
        /// конструктор создающий объект класса NMServer
        /// </summary>
        /// <param name="owner">визуальная форма, с потоком которой нужно синхронизировать вывод событий</param>
        public NMServer(Control owner)
        {
            IsRunning = false;
            m_Clients = ArrayList.Synchronized(new ArrayList());
            Owner = owner;

            m_SClientEdit = new Semaphore(1, 1);
        }

        /// <summary>
        /// конструктор создающий объект класса NMServer
        /// </summary>
        /// <param name="port">номер порта</param>
        /// <param name="owner">визуальная форма, с потоком которой нужно синхронизировать вывод событий</param>
        public NMServer(int port, Control owner) : this(owner)
        {
            Port = port;
        }

        /// <summary>
        /// деструктор, чтобы остановить потоки, если они были запущены
        /// </summary>
        ~NMServer()
        {
            if (IsRunning)
                StopServer();
        }

        /// <summary>
        /// список клиентов
        /// </summary>
        private ArrayList m_Clients;

        /// <summary>
        /// список клиентов
        /// </summary>
        public IList<ClientSocket> Clients
        {
            get
            {
                return Array.AsReadOnly<ClientSocket>((ClientSocket[])m_Clients.ToArray(typeof(ClientSocket)));
            }
        }

        /// <summary>
        /// обработка ошибок
        /// </summary>
        public event EventHandler<EventMsgArgs> Error;

        /// <summary>
        /// вывод события обработки ошибок
        /// </summary>
        /// <param name="msg">сообщение ошибки</param>
        private void ErrorEvent(string msg)
        {
            if (Owner != null && Owner.InvokeRequired)
            {
                Owner.Invoke(new Action<string>(ErrorEvent), msg);
            }
            else
                Error?.Invoke(this, new EventMsgArgs(msg));
        }

        /// <summary>
        /// отправляет данные клиенту по указанному адресу
        /// </summary>
        /// <param name="Address">адрес</param>
        /// <param name="data">данные</param>
        private void Write(int Address, byte[] data)
        {
            var i = m_Clients.Count - 1;
            try
            {
                while (IsRunning && (i >= 0) && (((ClientSocket)m_Clients[i]).Id != Address))
                    i--;
                if (IsRunning && (i >= 0) && ((ClientSocket)m_Clients[i]).Socket.Connected)
                    ((ClientSocket)m_Clients[i]).Send(data);
            }
            catch
            {
                (new Action<ClientSocket>(DeleteClient)).BeginInvoke(((ClientSocket)m_Clients[i]), null, null);
            }
        }

        /// <summary>
        /// время, по истечении которого идет проверка - жив ли клиент
        /// </summary>
        private int m_LiveTime = 0;

        /// <summary>
        /// время, по истечении которого идет проверка - жив ли клиент
        /// </summary>
        public int LiveTime
        {
            get
            {
                return m_LiveTime;
            }
            set
            {
                if (LiveTime == value)
                    return;

                if (value < 0)
                    throw new ArgumentException();

                m_LiveTime = value;

                LiveTimeChanged?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Событие, возникающе когда LiveTime изменено
        /// </summary>
        public event EventHandler LiveTimeChanged;

        /// <summary>
        /// проверка, жив ли клиент
        /// </summary>
        private void TestLive()
        {
            while (IsRunning)
            {
                if (LiveTime > 0 && m_Clients.Count > 0)
                {
                    if (Thread.CurrentThread.Priority != ThreadPriority.AboveNormal)
                        Thread.CurrentThread.Priority = ThreadPriority.AboveNormal;

                    var buf = new byte[8];
                    buf[0] = 0;
                    buf[1] = 0;
                    buf[2] = 0;
                    buf[3] = 0;
                    buf[4] = 0xFF;
                    buf[5] = 0xFF;
                    buf[6] = 0xFF;
                    buf[7] = 0x7F;
                    for (var I = 0; I < m_Clients.Count; I++)
                    {
                        ClientSocket CS = null;
                        try
                        {
                            CS = (ClientSocket)m_Clients[I];
                            CS.Send(buf);
                        }
                        catch
                        {
                            if (CS == null)
                                (new Action<ClientSocket>(DeleteClient)).BeginInvoke(CS, null, null);
                        }
                    }
                    Thread.Sleep(LiveTime);
                }
                else
                {
                    if (Thread.CurrentThread.Priority != ThreadPriority.Lowest)
                        Thread.CurrentThread.Priority = ThreadPriority.Lowest;
                    Thread.Sleep(1000);
                }
            }
        }

        /// <summary>
        /// true - сервер перезагружается
        /// </summary>
        private bool m_Restarting = false;

        /// <summary>
        /// перезагрузка сервера
        /// </summary>
        private void Restart()
        {
            m_Restarting = true;
            try
            {
                StopServer();

                if (ThreadRunServer.IsAlive)
                    ThreadRunServer.Abort();
                if (ThreadTestLive.IsAlive)
                    ThreadTestLive.Abort();

                StartServer();
            }
            finally
            {
                m_Restarting = false;
            }
        }

        /// <summary>
        /// Перезапуск сервера
        /// </summary>
        private Thread m_ThreadRestart = null;

        /// <summary>
        /// Чтение данных из сетевого потока
        /// </summary>
        /// <param name="ns">Сетевой поток откуда происходит чтение</param>
        /// <param name="size">Число байт, которое нужно прочтать</param>
        /// <returns></returns>
        private byte[] Read(NetworkStream ns, int size)
        {
            var tmp = new byte[size];
            var n = ns.Read(tmp, 0, size);
            var s = 0;
            while (n != size)
            {
                s += n;
                size = size - n;
                n = ns.Read(tmp, s, size);
            }

            return tmp;
        }

        /// <summary>
        /// Работа клиента
        /// </summary>
        /// <param name="client">текущий клиент</param>
        private void RunClient(ClientSocket client)
        {
            var ns = client.Socket.GetStream();
            try
            {
                while (IsRunning && client.Socket.Connected)
                {
                    var n = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);
                    if (IsRunning && client.Socket.Connected)
                    {
                        if (n == 0) //данные для сервера
                        {
                            //считывается команда
                            var Com = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);
                            if (IsRunning && client.Socket.Connected)
                            {
                                switch (Com)
                                {
                                    case -1://команда закрыть соединение
                                        (new Action<ClientSocket>(DeleteClient)).BeginInvoke(client, null, null);
                                        break;
                                    case 0x7FFFFFFF://команда перезагрузиться
                                        if ((m_ThreadRestart == null) || (!m_ThreadRestart.IsAlive))
                                        {
                                            m_ThreadRestart = new Thread(Restart);
                                            m_ThreadRestart.IsBackground = true;
                                            m_ThreadRestart.Start();
                                        }
                                        break;
                                }
                            }
                        }
                        else//пересылка данных
                        {
                            var addresses = new int[n];
                            var buf = Read(ns, (n + 1) * sizeof(int));
                            if (IsRunning && client.Socket.Connected)
                            {
                                for (var i = 0; i < n; i++)
                                    addresses[i] = BitConverter.ToInt32(buf, i * sizeof(int));
                                var size = BitConverter.ToInt32(buf, n * sizeof(int));
                                var tmp = Read(ns, size);
                                buf = new byte[size + 2 * sizeof(int)];
                                Array.Copy(tmp, 0, buf, 2 * sizeof(int), size);
                                if (IsRunning && client.Socket.Connected)
                                {
                                    Array.Copy(BitConverter.GetBytes(client.Id), 0, buf, 0, sizeof(int));
                                    Array.Copy(BitConverter.GetBytes(size), 0, buf, sizeof(int), sizeof(int));
                                    var i = 0;
                                    while (IsRunning && client.Socket.Connected && (i < n))
                                        Write(addresses[i++], buf);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                (new Action<ClientSocket>(DeleteClient)).BeginInvoke(client, null, null);
            }
        }

        /// <summary>
        /// семафор для синхронизации вставки и удаления клиентов
        /// </summary>
        private Semaphore m_SClientEdit;

        /// <summary>
        /// вызов события добавления нового клиента
        /// </summary>
        /// <param name="Id">номер клиента</param>
        /// <param name="Name">имя клиента</param>
        private void AddClientEvent(int Id, string Name)
        {
            if (Owner != null && Owner.InvokeRequired)
                Owner.Invoke(new Action<int, string>(AddClientEvent), Id, Name);
            else
                ClientAdded?.Invoke(this, new EventClientArgs(Id, Name));
        }

        /// <summary>
        /// событие добавления нового клиента
        /// </summary>
        public event EventHandler<EventClientArgs> ClientAdded;

        /// <summary>
        /// копирование данных
        /// </summary>
        /// <param name="source">откуда копируется</param>
        /// <param name="destination">куда копируется</param>
        /// <param name="index">номер последней свободной ячейки</param>
        private void CopyBytes(byte[] source, byte[] destination, ref int index)
        {

            Array.Copy(source, 0, destination, index, source.Length);
            index += source.Length;
        }

        /// <summary>
        /// добавляет новый клиент
        /// </summary>
        /// <param name="client">новый клиент</param>
        private void AddClient(ClientSocket client)
        {
            m_SClientEdit.WaitOne();
            try
            {
                m_Clients.Add(client);
                AddClientEvent(client.Id, client.Name);

                var data = new byte[4 * sizeof(int) + client.Name.Length * sizeof(char)];
                byte[] buf;
                var index = 0;
                CopyBytes(BitConverter.GetBytes(0), data, ref index);
                CopyBytes(BitConverter.GetBytes(1), data, ref index);
                CopyBytes(BitConverter.GetBytes(client.Id), data, ref index);
                CopyBytes(BitConverter.GetBytes(client.Name.Length), data, ref index);
                for (var i = 0; i < client.Name.Length; i++)
                    CopyBytes(BitConverter.GetBytes(client.Name[i]), data, ref index);

                for (var i = 0; i < m_Clients.Count; i++)
                {
                    if (((ClientSocket)m_Clients[i]).Id != client.Id)
                    {
                        try
                        {
                            //передаем сообщение, что добавлен новый клиент
                            ((ClientSocket)m_Clients[i]).Send(data);
                            //передаем сообщение о существующем клиенте новому клиенту
                            buf = new byte[4 * sizeof(int) + ((ClientSocket)m_Clients[i]).Name.Length * sizeof(char)];
                            index = 0;
                            CopyBytes(BitConverter.GetBytes((int)0), buf, ref index);
                            CopyBytes(BitConverter.GetBytes((int)1), buf, ref index);
                            CopyBytes(BitConverter.GetBytes(((ClientSocket)m_Clients[i]).Id), buf, ref index);
                            CopyBytes(BitConverter.GetBytes(((ClientSocket)m_Clients[i]).Name.Length), buf, ref index);
                            for (var j = 0; j < ((ClientSocket)m_Clients[i]).Name.Length; j++)
                                CopyBytes(BitConverter.GetBytes(((ClientSocket)m_Clients[i]).Name[j]), buf, ref index);
                            client.Send(buf);
                        }
                        catch
                        {
                            (new Action<ClientSocket>(DeleteClient)).BeginInvoke((ClientSocket)m_Clients[i], null, null);
                        }
                    }
                }
            }
            finally
            {
                m_SClientEdit.Release();
            }
        }

        /// <summary>
        /// событие удаления клиента
        /// </summary>
        public event EventHandler<EventClientArgs> ClientDeleted;

        /// <summary>
        /// удаление клиента
        /// </summary>
        /// <param name="id">номер клиента</param>
        /// <param name="name">название клиента</param>
        private void DeleteClientEvent(int id, string name)
        {
            if (Owner != null && Owner.InvokeRequired)
                Owner.Invoke(new Action<int, string>(DeleteClientEvent), id, name);
            else
                ClientDeleted?.Invoke(this, new EventClientArgs(id, name));
        }

        /// <summary>
        /// удаление клиента
        /// </summary>
        /// <param name="client">клиент</param>
        private void DeleteClient(ClientSocket client)
        {
            m_SClientEdit.WaitOne();
            try
            {
                if (m_Clients.IndexOf(client) >= 0)
                {
                    client.Socket.Close();

                    m_Clients.Remove(client);

                    int i;
                    var data = new byte[4 * sizeof(int) + client.Name.Length * sizeof(char)];
                    var index = 0;
                    CopyBytes(BitConverter.GetBytes((int)0), data, ref index);
                    CopyBytes(BitConverter.GetBytes((int)-1), data, ref index);
                    CopyBytes(BitConverter.GetBytes(client.Id), data, ref index);
                    CopyBytes(BitConverter.GetBytes(client.Name.Length), data, ref index);
                    for (i = 0; i < client.Name.Length; i++)
                        CopyBytes(BitConverter.GetBytes(client.Name[i]), data, ref index);

                    for (i = 0; i < m_Clients.Count; i++)
                        try
                        {
                            ((ClientSocket)m_Clients[i]).Send(data);
                        }
                        catch
                        {
                            (new Action<ClientSocket>(DeleteClient)).BeginInvoke((ClientSocket)m_Clients[i], null, null);
                        }

                    DeleteClientEvent(client.Id, client.Name);
                }
            }
            finally
            {
                m_SClientEdit.Release();
            }
        }

        /// <summary>
        /// добавляет нового клиента
        /// </summary>
        /// <param name="client">новый клиент</param>
        private void NewClient(TcpClient client)
        {
            ClientSocket clientSocket = null;
            client.ReceiveBufferSize = 0xFFFFFF;
            client.SendBufferSize = 0xFFFFFF;
            try
            {
                var ns = client.GetStream();
                if (IsRunning)
                {
                    var n = BitConverter.ToInt32(Read(ns, sizeof(int)), 0);

                    if (IsRunning)
                    {
                        var data = Read(ns, n * sizeof(char));
                        var name = "";
                        for (var i = 0; i < data.Length; i += 2)
                            name += BitConverter.ToChar(data, i);

                        clientSocket = new ClientSocket(name, client);
                        (new Action<ClientSocket>(RunClient)).BeginInvoke(clientSocket, null, null);
                        AddClient(clientSocket);
                    }
                }
            }
            catch
            {
                if (clientSocket != null)
                    (new Action<ClientSocket>(DeleteClient)).BeginInvoke(clientSocket, null, null);
            };
        }

        /// <summary>
        /// событие остановки клиента
        /// </summary>
        public event EventHandler Stoped;

        /// <summary>
        /// событие перезапуска клиента
        /// </summary>
        public event EventHandler Restarted;

        /// <summary>
        /// вызов события остановки клиента
        /// </summary>
        private void StopEvent()
        {
            if (Owner != null && Owner.InvokeRequired)
            {
                Owner.Invoke(new Action(StopEvent));
            }
            else
            {
                if (!m_Restarting)
                    Stoped?.Invoke(this, new EventArgs());
                else
                    Restarted?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// остановка сервера
        /// </summary>
        private void StopListener()
        {
            var buf = new byte[8];
            for (var i = 0; i < 7; i++)
                buf[i] = 0;
            //закрываются связи
            while (m_Clients.Count > 0)
            {
                try
                {
                    ((ClientSocket)m_Clients[0]).Send(buf);
                    ((ClientSocket)m_Clients[0]).Socket.Close();
                }
                catch { };
                m_Clients.RemoveAt(0);
            }
            //останов
            m_Listener.Stop();
            StopEvent();
        }

        /// <summary>
        /// сервер
        /// </summary>
        TcpListener m_Listener = null;

        /// <summary>
        /// работа сервера
        /// </summary>
        private void RunServer()
        {
            try
            {
                m_Listener = new TcpListener(IPAddress.Any, Port);
                m_Listener.Start();
                while (IsRunning)
                {
                    if (m_Listener.Pending())
                        (new Action<TcpClient>(NewClient)).BeginInvoke(m_Listener.AcceptTcpClient(), null, null);
                    else
                        Thread.Sleep(1000);
                }
                StopListener();
            }
            catch (Exception e)
            {
                if (m_Listener != null)
                    StopListener();
                ErrorEvent(e.Message);
            }
        }

        /// <summary>
        /// Поток для запуска серверного процесса
        /// </summary>
        private Thread ThreadRunServer;

        /// <summary>
        /// Поток для проверки активности клиентов
        /// </summary>
        private Thread ThreadTestLive;

        /// <summary>
        /// запуск работы сервера
        /// </summary>
        public void StartServer()
        {
            if (!IsRunning)
            {
                try
                {
                    //если есть "подвисшие" потоки, они прерываются
                    if ((ThreadRunServer != null) && ThreadRunServer.IsAlive)
                        try
                        {
                            ThreadRunServer.Abort();
                        }
                        catch { };
                    if ((ThreadTestLive != null) && ThreadTestLive.IsAlive)
                        try
                        {
                            ThreadTestLive.Abort();
                        }
                        catch { };

                    IsRunning = true;

                    ThreadRunServer = new Thread(RunServer);
                    ThreadRunServer.IsBackground = true;
                    ThreadRunServer.Start();

                    ThreadTestLive = new Thread(TestLive);
                    ThreadTestLive.IsBackground = true;
                    ThreadTestLive.Start();

                    Started?.Invoke(this, new EventArgs());
                }
                catch (Exception e)
                {
                    IsRunning = false;

                    if ((ThreadRunServer != null) && ThreadRunServer.IsAlive)
                        try
                        {
                            ThreadRunServer.Abort();
                        }
                        catch { };
                    if ((ThreadTestLive != null) && ThreadTestLive.IsAlive)
                        try
                        {
                            ThreadTestLive.Abort();
                        }
                        catch { };

                    throw new Exception(e.Message);
                }
            }
            else
                throw new Exception("Сервер уже запущен");
        }

        /// <summary>
        /// Событие, вызываемое когда сервер запущен
        /// </summary>
        public event EventHandler Started;

        /// <summary>
        /// остановка сервера
        /// </summary>
        public void StopServer()
        {
            if (IsRunning)
                IsRunning = false;
            else
                throw new Exception("Сервер уже остановлен");

        }
    }
}
