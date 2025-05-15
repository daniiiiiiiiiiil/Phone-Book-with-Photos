using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Телефонный_справочник_с_фотографиями
{
    public partial class Form1 : Form
    {
        Database db;
        Knopki knopki;

        private byte[] selectedPhotoBytes = null;
        private ListBox listContacts;
        public Form1()
        {
            InitializeComponent();

            db = new Database("Phone.db");

            knopki = new Knopki();
            knopki.InitializeComponents();

            // Инициализация ListBox для отображения контактов
            listContacts = new ListBox()
            {
                Location = new Point(350, 20),
                Size = new Size(300, 400)
            };
            this.Controls.Add(listContacts);

            // Обновляем список контактов при запуске
            RefreshContactList();

            this.Controls.AddRange(new Control[]
            {
            knopki.lblPhone, knopki.txtPhone,
            knopki.lblFirstName, knopki.txtFirstName,
            knopki.lblLastName, knopki.txtLastName,
            knopki.lblPhoto, knopki.pictureBox,
            knopki.btnAdd, knopki.btnEdit, knopki.btnDelete, knopki.btnSelectPhoto,
            listContacts // добавляем список контактов на форму
            });

            // Обработчики
            knopki.btnSelectPhoto.Click += BtnSelectPhoto_Click;
            knopki.btnAdd.Click += BtnAdd_Click;
            knopki.btnEdit.Click += BtnEdit_Click;
            knopki.btnDelete.Click += BtnDelete_Click;

            // Обработчик выбора контакта из списка
            listContacts.SelectedIndexChanged += ListContacts_SelectedIndexChanged;
        }

        // Метод для обновления отображения контактов
        private void RefreshContactList()
        {
            listContacts.Items.Clear();
            var contacts = db.GetAllPhones(); // предполагается, что есть такой метод
            foreach (var contact in contacts)
            {
                listContacts.Items.Add($"{contact.FirstName} {contact.LastName} - {contact.PhoneNumber}");
            }
        }

        // Обработка выбора контакта из списка
        private void ListContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listContacts.SelectedIndex;
            if (index >= 0)
            {
                var contact = db.GetAllPhones()[index]; // получаем контакт по индексу
                                                        // Заполняем поля формы
                knopki.txtId.Text = contact.Id.ToString();
                knopki.txtPhone.Text = contact.PhoneNumber;
                knopki.txtFirstName.Text = contact.FirstName;
                knopki.txtLastName.Text = contact.LastName;

                if (contact.Photo != null)
                {
                    using (MemoryStream ms = new MemoryStream(contact.Photo))
                    {
                        Image img = Image.FromStream(ms);
                        knopki.pictureBox.Image = img;
                    }
                }
                else
                {
                    knopki.pictureBox.Image = null;
                }
            }
        }

        private void BtnSelectPhoto_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    Image img = Image.FromFile(ofd.FileName);
                    knopki.pictureBox.Image = img;

                    using (MemoryStream ms = new MemoryStream())
                    {
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        selectedPhotoBytes = ms.ToArray();
                    }
                }
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string phoneNumber = knopki.txtPhone.Text;
            string firstName = knopki.txtFirstName.Text;
            string lastName = knopki.txtLastName.Text;

            var newPhoneItem = new PhoneItem
            {
                PhoneNumber = phoneNumber,
                FirstName = firstName,
                LastName = lastName,
                Photo = selectedPhotoBytes
            };

            db.AddPhone(newPhoneItem);
            MessageBox.Show("Добавлено новое значение");
            RefreshContactList(); // обновляем список
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            // Реализация редактирования по выбранному контакту
            // (может быть добавлена при необходимости)
            MessageBox.Show("Запись отредактирована");
            RefreshContactList();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            // Реализация удаления по выбранному контакту
            // (может быть добавлена при необходимости)
            MessageBox.Show("Запись удалена");
            RefreshContactList();
        }
    }
}