using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Windows.Media.Imaging;
using System.IO;
using System.Data;

namespace Дипломчик
{
    internal static class DBAdapter
    {
        private static SQLiteConnection _connection;
        public static User CurrentUser;

        public static void Connect(string filename)
        {
            _connection = new SQLiteConnection($"DATA SOURCE={filename};");
            _connection.Open();
        }

        public static User TryLogin(string login, string password)
        {
            string query = $"SELECT * FROM [Users] WHERE [Login] = {login} AND [Password] = {password};";
            SQLiteCommand command = new SQLiteCommand(query, _connection);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                CurrentUser = new User(reader);
                return CurrentUser;
            }
            else
            {
                return null;
            }
        }

        public static List<T> GetAll<T>(Func<SQLiteDataReader, SQLiteConnection, T> func, string tableName, string filter = "") where T:ABCModel
        {
            string query = $"SELECT * FROM [{tableName}] {filter};";
            SQLiteCommand command = new SQLiteCommand(query, _connection);
            SQLiteDataReader reader = command.ExecuteReader();

            List<T> result = new List<T>();
            while (reader.Read())
            {
                result.Add(func(reader, _connection));
            }
            return result;
        }

        public static void Update(ABCModel element)
        {
            string query = $"SELECT * FROM [{element.TableName}] WHERE [Id] = {element.Id}";
            SQLiteCommand command = new SQLiteCommand(query, _connection);
            if (command.ExecuteScalar() == null)
            {
                command = new SQLiteCommand(element.GetInsertString(), _connection);
            }
            else
            {
                command = new SQLiteCommand(element.GetUpdateString(), _connection);
            }
            command.ExecuteNonQuery();
        }

        public static void Delete(ABCModel element)
        {
            string query = $"DELETE FROM [{element.TableName}] WHERE [Id] == {element.Id};";
            SQLiteCommand command = new SQLiteCommand(query, _connection);
            command.ExecuteNonQuery();
        }
    }
}
