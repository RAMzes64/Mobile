using SQLite;

namespace cnsSQLite
{
    internal class Program
    {
        private static SQLiteConnection db;

        static void Main(string[] args)
        {
            db = new SQLiteConnection("myDB.db");
            db.CreateTable<Logs>();
            db.CreateTable<Users>();

            db.Insert(new Logs { DT = DateTime.Now });
            db.Insert(new Users { FIO = "Ваня", Email = "111@mail.ru", Age = 10 });

            foreach (var users in db.Table<Users>())
            {
                Console.WriteLine($" ID = {users.ID}, FIO = {users.FIO}");
            }
            //Console.WriteLine(string.Join(" | ", db.Table<Logs>().ToList()));
        }
    }
}
