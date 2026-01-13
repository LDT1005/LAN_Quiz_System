using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quiz.Client
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();

            // Tối ưu chống flicker
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.UserPaint, true);
            this.UpdateStyles();
            this.DoubleBuffered = true;

            // Căn giữa form, font hiện đại
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 10F);
            this.AcceptButton = btnLogin;

            // Giữ ảnh nền bạn chọn
            this.BackgroundImage = Image.FromFile("UTH.png"); // đổi tên nếu cần
            this.BackgroundImageLayout = ImageLayout.Stretch;

            // Style cho textbox
            txtUser.BorderStyle = BorderStyle.FixedSingle;
            txtUser.Text = "";

            txtPass.BorderStyle = BorderStyle.FixedSingle;
            txtPass.Text = "";
            txtPass.PasswordChar = '*';

            // Style cho button
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.BackColor = Color.SteelBlue;
            btnLogin.ForeColor = Color.White;
            btnLogin.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            // Label trạng thái
            lblStatus.ForeColor = Color.DarkRed;
            lblStatus.Text = "";

            // Làm label trong suốt
            label1.BackColor = Color.Transparent;
            label2.BackColor = Color.Transparent;
            lblStatus.BackColor = Color.Transparent;
            chkShowPass.BackColor = Color.Transparent;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUser.Text == "admin" && txtPass.Text == "123")
            {
                lblStatus.ForeColor = Color.Green;
                lblStatus.Text = "Đăng nhập thành công!";
                var dashboard = new DashboardForm(new NetworkClientService());
                dashboard.Show();
                this.Hide();
            }
            else
            {
                lblStatus.ForeColor = Color.Red;
                lblStatus.Text = "Sai tên đăng nhập hoặc mật khẩu!";
                MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtUser_TextChanged(object sender, EventArgs e)
        {
            // Có thể thêm kiểm tra realtime nếu cần
        }

        private void chkShowPass_CheckedChanged(object sender, EventArgs e)
        {
            txtPass.UseSystemPasswordChar = !chkShowPass.Checked;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void lblStatus_Click(object sender, EventArgs e)
        {

        }
    }
}
