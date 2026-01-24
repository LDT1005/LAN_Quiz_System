using Newtonsoft.Json;
using Quiz.Shared;
using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Quiz.Client.Model
{
    public partial class DashBoardForm : Form
    {
        private ClientConnection _connection;
        private bool _listening = true;

        // THÊM CONSTRUCTOR NÀY
        public DashBoardForm(ClientConnection connection)
        {
            InitializeComponent();
            _connection = connection;
        }

        // Constructor cũ (giữ lại để tránh lỗi Designer)
        public DashBoardForm()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile("UTH.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        private void DashBoardForm_Load(object sender, EventArgs e)
        {
            if (_connection == null) return;

            lblWelcome.Text = $"Xin chào, {_connection.StudentName}!";
            lblSystemStatus.Text = "✓ Đã kết nối server. Chờ giáo viên phát đề...";
            btnReady.Enabled = false;

            // Start listening for packets
            Task.Run(ListenForPackets);
        }

        private async Task ListenForPackets()
        {
            byte[] buffer = new byte[8192];
            StringBuilder messageBuffer = new StringBuilder();

            try
            {
                while (_listening && _connection.Client.Connected)
                {
                    int bytesRead = await _connection.Stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    messageBuffer.Append(Encoding.UTF8.GetString(buffer, 0, bytesRead));

                    int newlineIndex;
                    while ((newlineIndex = messageBuffer.ToString().IndexOf('\n')) >= 0)
                    {
                        string message = messageBuffer.ToString(0, newlineIndex).Trim();
                        messageBuffer.Remove(0, newlineIndex + 1);

                        if (!string.IsNullOrEmpty(message))
                        {
                            HandlePacket(message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() =>
                {
                    MessageBox.Show($"Mất kết nối server:\n{ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }));
            }
        }

        private void HandlePacket(string json)
        {
            try
            {
                var packet = JsonConvert.DeserializeObject<Packet>(json);

                this.Invoke(new Action(() =>
                {
                    if (packet.Type == Packet.TYPE_START_EXAM)
                    {
                        var examSession = JsonConvert.DeserializeObject<ExamSession>(packet.Data.ToString());
                        StartExam(examSession);
                    }
                    else if (packet.Type == Packet.TYPE_BROADCAST)
                    {
                        dynamic data = JsonConvert.DeserializeObject(packet.Data.ToString());
                        MessageBox.Show(data.Message.ToString(), "Thông Báo Từ Giáo Viên",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (packet.Type == Packet.TYPE_DISCONNECT)
                    {
                        MessageBox.Show("Giáo viên đã kết thúc phiên thi.", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"HandlePacket error: {ex.Message}");
            }
        }

        private void StartExam(ExamSession exam)
        {
            _listening = false;

            var examForm = new ExamFormV2(_connection, exam);
            examForm.FormClosed += (s, e) =>
            {
                this.Close();
            };

            this.Hide();
            examForm.ShowDialog();
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _listening = false;
            _connection?.Stream?.Close();
            _connection?.Client?.Close();
            base.OnFormClosing(e);
        }
    }
}