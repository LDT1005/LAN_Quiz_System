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

        public DashBoardForm(ClientConnection connection)
        {
            InitializeComponent();
            _connection = connection;
        }

        public DashBoardForm()
        {
            InitializeComponent();
            try
            {
                this.BackgroundImage = Image.FromFile("UTH.png");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch { }
        }

        private void DashBoardForm_Load(object sender, EventArgs e)
        {
            if (_connection == null) return;

            lblWelcome.Text = $"Xin chào, {_connection.StudentName}!";
            lblSystemStatus.Text = "✓ Đã kết nối server. Chờ giáo viên phát đề...";
            btnReady.Enabled = true; // Cho phép nhấn Sẵn sàng
        }

        // FIX: Chuyển sang đọc gói tin theo chuẩn 4-byte header
        private async Task ListenForPackets()
        {
            try
            {
                while (_listening && _connection.Client.Connected)
                {
                    // Sử dụng DataHelper để đọc gói tin đúng cấu trúc Server gửi
                    byte[] payload = await Task.Run(() => DataHelper.ReadPacket(_connection.Stream));

                    if (payload == null) break; // Server ngắt kết nối

                    string json = Encoding.UTF8.GetString(payload);

                    // Đưa việc xử lý về luồng giao diện
                    this.Invoke(new Action(() => HandlePacket(json)));
                }
            }
            catch (Exception ex)
            {
                this.Invoke(new Action(() => {
                    lblSystemStatus.Text = "✗ Mất kết nối hệ thống.";
                    lblSystemStatus.ForeColor = Color.Red;
                }));
            }
        }

        private void HandlePacket(string json)
        {
            try
            {
                var packet = JsonConvert.DeserializeObject<Packet>(json);

                if (packet.Type == Packet.TYPE_START_EXAM)
                {
                    var examSession = JsonConvert.DeserializeObject<ExamSession>(packet.Data.ToString());
                    StartExam(examSession);
                }
                else if (packet.Type == Packet.TYPE_BROADCAST)
                {
                    // Xử lý thông báo Multicast từ giáo viên
                    dynamic data = JsonConvert.DeserializeObject(packet.Data.ToString());
                    MessageBox.Show(data.Message.ToString(), "Thông Báo Khẩn",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (packet.Type == Packet.TYPE_DISCONNECT)
                {
                    MessageBox.Show("Phiên thi đã kết thúc.", "Thông báo");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi xử lý gói tin: {ex.Message}");
            }
        }

        private void StartExam(ExamSession exam)
        {
            _listening = false; // Ngừng nghe tại Dashboard để chuyển sang Form làm bài

            var examForm = new ExamFormV2(_connection, exam);
            examForm.FormClosed += (s, e) => this.Close();

            this.Hide();
            examForm.ShowDialog();
        }

        private void btnReady_Click(object sender, EventArgs e)
        {
            // BẮT ĐẦU NGHE: Khi học sinh nhấn Sẵn sàng thì mới bắt đầu nhận đề
            btnReady.Enabled = false;
            btnReady.Text = "Đang đợi đề...";
            lblSystemStatus.Text = "● Đang lắng nghe tín hiệu phát đề...";

            Task.Run(ListenForPackets);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _listening = false;
            base.OnFormClosing(e);
        }
    }
}