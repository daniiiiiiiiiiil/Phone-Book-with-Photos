using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Телефонный_справочник_с_фотографиями
{
    internal class Database
    {
        private SQLiteConnection connection;

        public Database(string dbPath)
        {
            connection = new SQLiteConnection($"Data Source={dbPath};Version=3");
            connection.Open();
            CreateDataBase();
        }

        public void CreateDataBase()
        {
            try
            {
                string create = @"CREATE TABLE IF NOT EXISTS Phone(
                                 ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                 PhoneNumber TEXT NOT NULL,
                                 FirstName TEXT NOT NULL,
                                 LastName TEXT,
                                 Photo BLOB)";
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
                            string phoneNumber = reader["PhoneNumber"].ToString();
                            string firstName = reader["FirstName"].ToString();
                            string lastName = reader["LastName"] != DBNull.Value ? reader["LastName"].ToString() : "";

                            byte[] photo = null;
                            if (reader["Photo"] != DBNull.Value)
                            {
                                photo = (byte[])reader["Photo"];
                            }

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
                string add = @"INSERT INTO Phone(PhoneNumber, FirstName, LastName, Photo) VALUES (@Num, @Fname, @Lname, @Pho)";
                using (var command = new SQLiteCommand(add, connection))
                {
                    command.Parameters.AddWithValue("@Num", phoneItem.PhoneNumber);
                    command.Parameters.AddWithValue("@Fname", phoneItem.FirstName);
                    command.Parameters.AddWithValue("@Lname", phoneItem.LastName);
                    command.Parameters.AddWithValue("@Pho", (object)phoneItem.Photo ?? DBNull.Value);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Контакт успешно добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении: {ex.Message}");
            }
        }
        public void EditPhone(PhoneItem phoneItem)
        {
            try
            {
                string update = @"UPDATE Phone SET 
                        PhoneNumber = @Num,
                        FirstName = @Fname,
                        LastName = @Lname,
                        Photo = @Pho
                        WHERE ID = @Id";

                using (var command = new SQLiteCommand(update, connection))
                {
                    command.Parameters.AddWithValue("@Num", phoneItem.PhoneNumber);
                    command.Parameters.AddWithValue("@Fname", phoneItem.FirstName);
                    command.Parameters.AddWithValue("@Lname", phoneItem.LastName);
                    command.Parameters.AddWithValue("@Pho", (object)phoneItem.Photo ?? DBNull.Value);
                    command.Parameters.AddWithValue("@Id", phoneItem.Id);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Контакт успешно обновлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при редактировании контакта: {ex.Message}");
            }
        }

        public void DeletePhone(int id)
        {
            try
            {
                string delete = "DELETE FROM Phone WHERE ID = @Id";

                using (var command = new SQLiteCommand(delete, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Контакт успешно удален!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении контакта: {ex.Message}");
            }
        }
    }
}
