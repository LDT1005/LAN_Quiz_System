using Newtonsoft.Json;
using Quiz.Shared;
using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using System.Drawing;


namespace Quiz.Client.Model
{
    public partial class LoginForm : Form
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public LoginForm()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile("UTH.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtServerIP.Text = "127.0.0.1";
            lblStatus.Text = "Nhập thông tin để kết nối";
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentID.Text) ||
                string.IsNullOrWhiteSpace(txtStudentName.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnLogin.Enabled = false;
            lblStatus.Text = "Đang kết nối...";

            try
            {
                _client = new TcpClient();
                await _client.ConnectAsync(txtServerIP.Text, 8888);
                _stream = _client.GetStream();

                var loginData = new LoginData
                {
                    StudentID = txtStudentID.Text.Trim(),
                    StudentName = txtStudentName.Text.Trim()
                };

                var packet = new Packet(Packet.TYPE_LOGIN, loginData.StudentID, loginData);
                string json = JsonConvert.SerializeObject(packet) + "\n";
                byte[] dataBytes = Encoding.UTF8.GetBytes(json); // SỬA: Đổi tên biến

                await _stream.WriteAsync(dataBytes, 0, dataBytes.Length); // SỬA
                await _stream.FlushAsync();

                lblStatus.Text = "Đợi phản hồi từ server...";

                byte[] buffer = new byte[4096];
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

                var responsePacket = JsonConvert.DeserializeObject<Packet>(response);

                if (responsePacket.Type == Packet.TYPE_LOGIN_SUCCESS)
                {
                    lblStatus.Text = "Đăng nhập thành công!";

                    var connection = new ClientConnection
                    {
                        Client = _client,
                        Stream = _stream,
                        StudentID = loginData.StudentID,
                        StudentName = loginData.StudentName
                    };

                    this.Tag = connection;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else if (responsePacket.Type == Packet.TYPE_LOGIN_FAILED)
                {
                    dynamic responseData = JsonConvert.DeserializeObject(responsePacket.Data.ToString()); // SỬA: Đổi tên biến
                    MessageBox.Show($"Đăng nhập thất bại:\n{responseData.Reason}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);

                    lblStatus.Text = "Đăng nhập thất bại";
                    btnLogin.Enabled = true;

                    _stream?.Close();
                    _client?.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                lblStatus.Text = "Kết nối thất bại";
                btnLogin.Enabled = true;

                _stream?.Close();
                _client?.Close();
            }
        }
    }
}