using Microsoft.Data.Sqlite;
using System.Xml.Linq;

namespace cnsSQLite_ms
{
    internal class Program
    {
        private static SqliteConnection db;

        static void Main(string[] args)
        {
            db = new SqliteConnection("Data Source=myDB.db");
            db.Open();
            CreateTables(db);
            InsertUser(db, "Ваня", 20);

            printUsers(db);
        }

        private static void printUsers(SqliteConnection db)
        {
            var sql = "SELECT id, name, age FROM Users;";
            using var command = new SqliteCommand(sql, db);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var id = reader.GetInt32(0);
                var name = reader.GetString(1);
                var age = reader.GetInt32(2);

                Console.WriteLine($"id = {id} | name = {name} | age = {age}");
            }
        }

        private static void InsertUser(SqliteConnection db, string name, int age)
        {
            var sql = "INSERT Into Users (name, age) VALUES (@name, @age);";
            using var command = new SqliteCommand(sql, db);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@age", age);
            command.ExecuteNonQuery();
        }

        private static void CreateTables(SqliteConnection db)
        {
            var sql = "CREATE TABLE IF NOT EXISTS Users (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT UNIQUE, name varchar(50) NOT NULL, age INTEGER NOT NULL)";
            using var command = new SqliteCommand(sql, db);
            command.ExecuteNonQuery();
        }
    }
}
