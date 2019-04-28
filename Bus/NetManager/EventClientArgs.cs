using System;

namespace NetManager
{
    /// <summary>
    /// Аргументы события от клиента
    /// </summary>
    public class EventClientArgs : EventArgs
    {
        /// <summary>
        /// Id клиента
        /// </summary>
        public int ClientId
        {
            get;
            private set;
        }

        /// <summary>
        /// Имя клиента
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Id клиента</param>
        /// <param name="name">имя клиента</param>
        public EventClientArgs(int id, string name)
        {
            ClientId = id;
            this.Name = name;
        }
    }
}
