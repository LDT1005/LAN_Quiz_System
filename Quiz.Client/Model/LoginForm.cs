using System;
using System.Windows.Forms;

namespace Quiz.Client
{
    public partial class LoginForm : Form
    {
        private ClientConnection _connection = new ClientConnection();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                _connection.Connect();
                _connection.SendString(txtUser.Text); // Đã sửa txtMSSV -> txtUser

                this.Tag = _connection; // Lưu lại để mang sang Dashboard
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.UseSystemPasswordChar = !chkShowPass.Checked; // Đã sửa txtPassword -> txtPass
        }
    }
}