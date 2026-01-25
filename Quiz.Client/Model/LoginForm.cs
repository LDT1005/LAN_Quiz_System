using Newtonsoft.Json;
using Quiz.Shared;
using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz.Client.Model
{
    public partial class LoginForm : Form
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public LoginForm()
        {
            InitializeComponent();

            // Ép nút bấm lên lớp trên cùng để tránh bị ảnh nền che mất Z-Order
            this.btnLogin.BringToFront();

            try
            {
                // Đường dẫn file ảnh phải nằm trong bin/Debug
                this.BackgroundImage = Image.FromFile("UTH.png");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch { /* Bỏ qua nếu thiếu ảnh để tránh Crash */ }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtServerIP.Text = "127.0.0.1";
            lblStatus.Text = "Nhập thông tin để kết nối";

            // Gợi ý thông tin cá nhân của bạn để tiện test nhanh
            txtStudentID.Text = "0752050026156";
            txtStudentName.Text = "Lại Đức Thành";
        }

        // ĐỔI TÊN THÀNH btnLogin_Click_1 ĐỂ KHỚP VỚI FILE DESIGNER CỦA BẠN
        private async void btnLogin_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentID.Text) || string.IsNullOrWhiteSpace(txtStudentName.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Nhắc nhở", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnLogin.Enabled = false;
            lblStatus.Text = "Đang kết nối tới máy chủ...";

            try
            {
                _client = new TcpClient();
                // Kết nối tới Port 8888 chuẩn của ServerManager
                await _client.ConnectAsync(txtServerIP.Text, 8888);
                _stream = _client.GetStream();

                var loginData = new LoginData
                {
                    StudentID = txtStudentID.Text.Trim(),
                    StudentName = txtStudentName.Text.Trim()
                };

                // GỬI GÓI TIN CHUẨN (Sử dụng 4-byte length header từ DataHelper)
                var packet = new Packet(Packet.TYPE_LOGIN, loginData.StudentID, loginData);
                DataHelper.SendObject(_stream, packet);

                lblStatus.Text = "Đang đợi phản hồi từ Server...";

                // ĐỌC PHẢN HỒI CHUẨN
                byte[] responseBytes = await Task.Run(() => DataHelper.ReadPacket(_stream));
                if (responseBytes == null) throw new Exception("Máy chủ đã ngắt kết nối đột ngột.");

                string responseJson = Encoding.UTF8.GetString(responseBytes);
                var responsePacket = JsonConvert.DeserializeObject<Packet>(responseJson);

                if (responsePacket.Type == Packet.TYPE_LOGIN_SUCCESS)
                {
                    lblStatus.Text = "Đăng nhập thành công!";

                    // Lưu thông tin kết nối vào Tag để Program.cs có thể lấy dữ liệu
                    var connection = new ClientConnection
                    {
                        Client = _client,
                        Stream = _stream,
                        StudentID = loginData.StudentID,
                        StudentName = loginData.StudentName
                    };

                    this.Tag = connection;
                    this.DialogResult = DialogResult.OK; // Kích hoạt lệnh chuyển sang màn hình Waiting
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Đăng nhập thất bại. Vui lòng kiểm tra lại MSSV!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnLogin.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể kết nối: {ex.Message}", "Lỗi mạng", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Kết nối thất bại";
                btnLogin.Enabled = true;
                _client?.Close();
            }
        }

        // Stub để tránh lỗi Designer nếu bạn lỡ click vào TextBox
        private void textBox2_TextChanged(object sender, EventArgs e) { }
    }
}