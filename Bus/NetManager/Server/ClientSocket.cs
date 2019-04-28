using System.Net.Sockets;
using System.Threading;
using NetManager.Client;

namespace NetManager.Server
{
    /// <summary>
    /// Класс, в котором реализована синхронная передача данных клиенту
    /// </summary>
    public class ClientSocket : ClientAddress
    {
        /// <summary>
        /// Для генерации Id
        /// </summary>
        private static int lastId = 0;

        /// <summary>
        /// Сокет для подключения
        /// </summary>
        public TcpClient Socket
        {
            get;
            private set;
        }

        /// <summary>
        /// Семафор для синхронизации отправки данных клиенту
        /// </summary>
        private Semaphore m_SWrite;

        /// <summary>
        /// Отправка данных клиенту
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte[] data)
        {
            m_SWrite.WaitOne();
            try
            {
                var ns = Socket.GetStream();
                ns.Write(data, 0, data.Length);
            }
            finally
            {
                m_SWrite.Release();
            }
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="name">Название клиента</param>
        /// <param name="socket">Узел связи</param>
        public ClientSocket(string name, TcpClient socket) : base(++lastId, name)
        {
            Socket = socket;
            m_SWrite = new Semaphore(1, 1);
        }
    }
}