using SQLite;

namespace cnsSQLite
{
    internal class Users
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set;  }
        public string FIO { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}