using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Телефонный_справочник_с_фотографиями
{
    public partial class Form1 : Form
    {
        private Database db;
        private Knopki knopki;

        private byte[] selectedPhotoBytes = null;

        public Form1()
        {
            InitializeComponent();

            db = new Database("Phone.db"); 

            knopki = new Knopki();
            knopki.InitializeComponents();
            var listContacts = new ListBox()
            {
                Location = new Point(350, 20),
                Size = new Size(300, 400)
            };
            this.Controls.Add(listContacts);
            this.listContacts = listContacts; 

            RefreshContactList();

            this.Controls.AddRange(new Control[]
            {
                knopki.lblPhone, knopki.txtPhone,
                knopki.lblFirstName, knopki.txtFirstName,
                knopki.lblLastName, knopki.txtLastName,
                knopki.lblPhoto, knopki.pictureBox,
                knopki.btnAdd, knopki.btnEdit, knopki.btnDelete, knopki.btnSelectPhoto,
                listContacts
            });
            knopki.btnSelectPhoto.Click += BtnSelectPhoto_Click;
            knopki.btnAdd.Click += BtnAdd_Click;
            knopki.btnEdit.Click += BtnEdit_Click;
            knopki.btnDelete.Click += BtnDelete_Click;
            listContacts.SelectedIndexChanged += ListContacts_SelectedIndexChanged;
        }

        private ListBox listContacts;

        private void RefreshContactList()
        {
            listContacts.Items.Clear();
            var contacts = db.GetAllPhones();
            foreach (var contact in contacts)
            {
                listContacts.Items.Add($"{contact.FirstName} {contact.LastName} - {contact.PhoneNumber}");
            }
        }

        private void ListContacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listContacts.SelectedIndex;
            if (index >= 0)
            {
                var contacts = db.GetAllPhones();
                if (index < contacts.Count)
                {
                    var contact = contacts[index];

                    knopki.txtId.Text = contact.Id.ToString();
                    knopki.txtPhone.Text = contact.PhoneNumber;
                    knopki.txtFirstName.Text = contact.FirstName;
                    knopki.txtLastName.Text = contact.LastName;

                    if (contact.Photo != null && contact.Photo.Length > 0)
                    {
                        try
                        {
                            using (MemoryStream ms = new MemoryStream(contact.Photo))
                            {
                                knopki.pictureBox.Image = Image.FromStream(ms);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}");
                            knopki.pictureBox.Image = null;
                        }
                    }
                    else
                    {
                        knopki.pictureBox.Image = null;
                    }
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
            RefreshContactList();
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Запись отредактирована");
            RefreshContactList();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Запись удалена");
            RefreshContactList();
        }
    }
}
