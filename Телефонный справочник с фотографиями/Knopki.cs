using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Телефонный_справочник_с_фотографиями
{
    internal class Knopki
    {
        public Label lblPhone, lblFirstName, lblLastName, lblPhoto;
        public TextBox txtId, txtPhone, txtFirstName, txtLastName;
        public PictureBox pictureBox;
        public Button btnAdd, btnEdit, btnDelete, btnSelectPhoto;
        public void InitializeComponents()
        {
            lblPhone = new Label() { Text = "Телефон:", Location = new Point(20, 60), AutoSize = true };
            lblFirstName = new Label() { Text = "Имя:", Location = new Point(20, 100), AutoSize = true };
            lblLastName = new Label() { Text = "Фамилия:", Location = new Point(20, 140), AutoSize = true };
            lblPhoto = new Label() { Text = "Фото:", Location = new Point(20, 220), AutoSize = true };

            txtId = new TextBox() { Location = new Point(100, 20), Width = 200 };
            txtPhone = new TextBox() { Location = new Point(100, 60), Width = 200 };
            txtFirstName = new TextBox() { Location = new Point(100, 100), Width = 200 };
            txtLastName = new TextBox() { Location = new Point(100, 140), Width = 200 };

            pictureBox = new PictureBox()
            {
                Location = new Point(100, 200),
                Size = new Size(150, 150),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom
            };

            btnAdd = new Button()
            {
                Text = "Добавить",
                Location = new Point(20, 350),
                Width = 100
            };
            btnEdit = new Button()
            {
                Text = "Редактировать",
                Location = new Point(140, 350),
                Width = 100
            };
            btnDelete = new Button()
            {
                Text = "Удалить",
                Location = new Point(260, 350),
                Width = 100
            };
            btnSelectPhoto = new Button()
            {
                Text = "Выбрать фото",
                Location = new Point(260, 220),
                Width = 100
            };

            
        }

    }
}
