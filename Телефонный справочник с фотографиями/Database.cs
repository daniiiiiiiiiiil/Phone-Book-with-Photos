using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Телефонный_справочник_с_фотографиями
{
    internal class Database
    {
        private SQLiteConnection connection;
        public Database(string db)
        {
            connection = new SQLiteConnection($"Data Source={db};Vercion=3");
            connection.Open();
        }
        private void CreateDataBase()
        {
            try
            {
                string create = @"CREATE TABLE IF NOT EXISTS Phone(
                                 ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                 PhoneNumber Text Not Null,
                                 FirstName Text Not Null,
                                 LastName Text,
                                 Photo text)";
                Execute(create);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании таблицы: {ex.Message}");
            }
            
        }
        private void Execute(string query)
        {
            using (var command = new SQLiteCommand(query, connection))
            {
                command.ExecuteNonQuery();
            }
        }
        public List<PhoneItem> GetAllPhones()
        {
            List<PhoneItem> phones = new List<PhoneItem>();
            string query = "SELECT ID, PhoneNumber, FirstName, LastName, Photo FROM Phone";

            try
            {
                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string phoneNumber = reader["PhoneNumber"] != DBNull.Value ? reader["PhoneNumber"].ToString() : "";
                            string firstName = reader["FirstName"] != DBNull.Value ? reader["FirstName"].ToString() : "";
                            string lastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : "";
                            byte[] photo = reader["Photo"] != DBNull.Value ? (byte[])reader["Photo"] : null;

                            phones.Add(new PhoneItem(id, phoneNumber, firstName, lastName, photo));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при получении данных: {ex.Message}");
            }

            return phones;
        }

        public void AddPhone(PhoneItem phoneItem)
        {
            try
            {
                string add = @"INSERT INTO Phone(PhoneNumber,FirstName,LastName,Photo) VALUES (@Num,@Fname,@Lname,@Pho)";
                using (var connection = new SQLiteConnection("Data Source=Phone.db"))
                {
                    connection.Open();
                    using (var command = new SQLiteCommand(add, connection))
                    {
                        command.Parameters.AddWithValue("@Num", phoneItem.PhoneNumber);
                        command.Parameters.AddWithValue("@Fname", phoneItem.FirstName);
                        command.Parameters.AddWithValue("@Lname", phoneItem.LastName);
                        command.Parameters.AddWithValue("@Pho", phoneItem.Photo ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }
                MessageBox.Show("Контакт успешно добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении задачи: {ex.Message}");
            }
        }
    }
}

