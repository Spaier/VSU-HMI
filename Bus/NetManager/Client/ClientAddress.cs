namespace NetManager.Client
{
    /// <summary>
    /// Класс содержащий адрес и имя клиента
    /// </summary>
    public class ClientAddress
    {
        /// <summary>
        /// Адрес клиента
        /// </summary>
        public int Id
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
        /// <param name="id">Адрес клиента</param>
        /// <param name="name">Имя клиента</param>
        public ClientAddress(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString()
        {
            return Name + " (" + Id.ToString() + ")";
        }
    }
}
