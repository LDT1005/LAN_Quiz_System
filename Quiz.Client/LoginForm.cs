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
                // FIX LỖI CS0103: Sử dụng txtUser thay vì txtMSSV
                _connection.SendString(txtUser.Text);

                this.Tag = _connection; // Lưu lại để Program.cs mang sang Dashboard
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi: " + ex.Message); }
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            // FIX LỖI CS0103: Sử dụng txtPass thay vì txtPassword
            txtPass.UseSystemPasswordChar = !chkShowPass.Checked;
        }
    }
}