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
            try
            {
                this.BackgroundImage = Image.FromFile("UTH.png");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch { }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtServerIP.Text = "127.0.0.1";
            lblStatus.Text = "Nhập thông tin để kết nối";
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtStudentID.Text) || string.IsNullOrWhiteSpace(txtStudentName.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                // GỬI GÓI TIN CHUẨN (4-byte length prefix)
                var packet = new Packet(Packet.TYPE_LOGIN, loginData.StudentID, loginData);
                DataHelper.SendObject(_stream, packet);

                lblStatus.Text = "Đợi phản hồi từ server...";

                // ĐỌC PHẢN HỒI CHUẨN
                byte[] responseBytes = await Task.Run(() => DataHelper.ReadPacket(_stream));
                if (responseBytes == null) throw new Exception("Server ngắt kết nối.");

                string responseJson = Encoding.UTF8.GetString(responseBytes);
                var responsePacket = JsonConvert.DeserializeObject<Packet>(responseJson);

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
                else
                {
                    MessageBox.Show("Đăng nhập thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnLogin.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnLogin.Enabled = true;
                _client?.Close();
            }
        }

        // Stub để tránh lỗi Designer
        private void textBox2_TextChanged(object sender, EventArgs e) { }
    }
}