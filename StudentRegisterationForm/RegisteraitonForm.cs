using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StudentRegisterationForm
{
    public partial class frmRegisteration : Form
    {
        public frmRegisteration()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmRegisteration_Load(object sender, EventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Name is required!","Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return;
            }
            if (!Regex.IsMatch(txtEmail.Text.Trim(), @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                MessageBox.Show("Enter a valid email address!","Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEmail.Focus();
                return;
            }
            if(txtPassword.Text.Length < MaxNumberPass.Password_length)
            {
                MessageBox.Show($"Password must be at least {MaxNumberPass.Password_length}characters!",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }
            if(!rdoMale.Checked && !rdoFemale.Checked && !rdoOther.Checked)
            {
                MessageBox.Show("Please select a gender!","Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if(cmbCountry.SelectedItem == null)
            {
                MessageBox.Show("Please select a country!","Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCountry.Focus();
                return;
            }
            if(lblSelectedColor.Text == "No Color Selected")
            {
                MessageBox.Show("Please select your favorite color!","Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return ;
            }

            string name = txtName.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string gender = rdoMale.Checked ? "Male" : rdoFemale.Checked ? "Female" : "Other";
            string birthdate = dtpBirthdate.Value.ToString();
            string country = cmbCountry.SelectedItem?.ToString() ?? "Not Selected";
            string color = lblSelectedColor.Text.Replace("Selected Color:", "");
            lblResult.Text = $"Name: {name} \nEmail:{email} \nGender:{gender} \n" + $"Birthdate:{birthdate} \n" +
                $"Country:{country} \nFavorite Color:{color}";
        }

        private void btnPickColor_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                lblSelectedColor.Text = $"Selected Color: {colorDialog.Color.Name}";
            }

        }

        private void cmbCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            txtName.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";

            rdoMale.Checked = false;
            rdoFemale.Checked = false;
            rdoOther.Checked = false;

            cmbCountry.SelectedIndex = -1;

            dtpBirthdate.Value = DateTime .Now;

            lblSelectedColor.Text = "No Color Selected";

            lblResult.Text = "";

            picStudent.Image = null;

            txtName.Focus();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";

            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                picStudent.Image = Image.FromFile(openFileDialog.FileName);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Please fill in at least Name and  Email before saving!","Validation Errror",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string data = $"{txtName.Text}\n" +
                $"{txtEmail.Text}\n" +
                $"{txtPassword.Text}\n"+
                $"{(rdoMale.Checked ? "Male" : rdoFemale.Checked ? "Female" : "Other")}\n" +
            $"{dtpBirthdate.Value.ToString("yyyy-MM-dd")}\n" +
            $"{cmbCountry.SelectedItem?.ToString()}\n" +
            $"{lblSelectedColor.Text.Replace("Selected Color:", "")}\n" +
            $"{(picStudent.Image != null ? "student_picture.jpg" : "No Image")}\n";

            File.WriteAllText("student_data.txt", data);

            if(picStudent.Image != null)
            {
                picStudent.Image.Save("student_picture.jpg");
            }

            MessageBox.Show("Data saved successfully!", "Success",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (!File.Exists("student_data.txt"))
            {
                MessageBox.Show("No saved data found!","Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] lines = File.ReadAllLines("student_data.txt");

            if(lines.Length <7)
            {
                MessageBox.Show("Saved data is incomplete or corrupted!","Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            } 

            txtName.Text = lines[0];
            txtEmail.Text = lines[1];
            txtPassword.Text = lines[2];

            if (lines[3] == "Male") rdoMale.Checked = true;
            else if (lines[3] == "Female") rdoFemale.Checked = true;
            else rdoOther.Checked = true;

            string rawDate = lines[4].Trim();
            dtpBirthdate.Value = DateTime.ParseExact(rawDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            cmbCountry.SelectedItem = lines[5];
            lblSelectedColor.Text = "Selected Color:" + lines[6];

            if (File.Exists("student_picture.jpg") && lines[7] == "student_picture.jpg")
            {
                picStudent.Image = Image.FromFile("student_picture.jpg");
            }
            else
            { 
                picStudent.Image = null;
            }

            MessageBox.Show("Data loaded successfully!","Success",
                MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
    }
}
   