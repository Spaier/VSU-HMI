using System;

namespace NetManager
{
    /// <summary>
    /// Класс для аргументов собятия передачи сообщения
    /// </summary>
    public class EventMsgArgs : EventArgs
    {
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Msg
        {
            get;
            private set;
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="message">Сообщение</param>
        public EventMsgArgs(string message)
        {
            Msg = message;
        }
    }
}
