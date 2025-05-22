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
            if (listContacts.SelectedIndex >= 0 && !string.IsNullOrEmpty(knopki.txtId.Text))
            {
                try
                {
                    var updatedContact = new PhoneItem
                    {
                        Id = int.Parse(knopki.txtId.Text),
                        PhoneNumber = knopki.txtPhone.Text,
                        FirstName = knopki.txtFirstName.Text,
                        LastName = knopki.txtLastName.Text,
                        Photo = selectedPhotoBytes
                    };

                    db.EditPhone(updatedContact);
                    RefreshContactList();

                    selectedPhotoBytes = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при редактировании: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите контакт для редактирования");
            }
        }
        private void ClearFields()
        {
            knopki.txtId.Clear();
            knopki.txtPhone.Clear();
            knopki.txtFirstName.Clear();
            knopki.txtLastName.Clear();
            knopki.pictureBox.Image = null;
            selectedPhotoBytes = null;
        }
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (listContacts.SelectedIndex >= 0 && !string.IsNullOrEmpty(knopki.txtId.Text))
            {
                try
                {
                    int id = int.Parse(knopki.txtId.Text);

                    if (MessageBox.Show("Вы уверены, что хотите удалить этот контакт?",
                        "Подтверждение удаления",
                        MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        db.DeletePhone(id);
                        RefreshContactList();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите контакт для удаления");
            }
        }
    }
}
