using System;

namespace NetManager
{
    /// <summary>
    /// Аргументы события получения данных от клиента
    /// </summary>
    public class EventClientMsgArgs : EventClientArgs
    {
        /// <summary>
        /// Данные от клиента
        /// </summary>
        public byte[] Msg
        {
            get;
            private set;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="id">Id клиента</param>
        /// <param name="name">Название клиента</param>
        /// <param name="data">Данные от клиента</param>
        public EventClientMsgArgs(int id, string name, byte[] data) : base(id, name)
        {
            Msg = data;
        }
    }
}
