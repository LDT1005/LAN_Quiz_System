using Newtonsoft.Json;
using Quiz.Server.Network;
using Quiz.Shared;
using System;
using System.Windows.Forms;
using System.Drawing;

namespace Quiz.Server
{
    public partial class Form1 : Form
    {
        private ServerManager _server = new ServerManager();
        private ExamSession _currentExam;

        public Form1()
        {
            InitializeComponent();
            _server.OnClientCountChanged += c => Invoke(new Action(() => lblClients.Text = "Clients: " + c));
            _server.OnAnswerReceived += s => Invoke(new Action(() => txtLog.AppendText($"[{DateTime.Now:HH:mm}] SV {s.StudentID} đã nộp bài.\r\n")));
            // Gán ảnh logo
            picLogo.Image = Image.FromFile("logo.png");
            picLogo.SizeMode = PictureBoxSizeMode.Zoom;

            // Đặt vị trí góc phải trên cùng
            picLogo.Location = new Point(this.ClientSize.Width - picLogo.Width - 10, 10);

            // Giữ cố định khi resize Form
            picLogo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.BackgroundImage = Image.FromFile("White.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;


        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "JSON|*.json" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _currentExam = JsonConvert.DeserializeObject<ExamSession>(System.IO.File.ReadAllText(ofd.FileName));
                    txtLog.AppendText($"Đã nhận đề: {_currentExam.ExamTitle}\r\n");
                }
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (_currentExam != null) _server.BroadcastExam(_currentExam);
        }

        private void Form1_Load(object sender, EventArgs e) => _server.Start(8888);

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtNotice_TextChanged(object sender, EventArgs e)
        {
            

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtLog_TextChanged(object sender, EventArgs e)
        {

        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Image bg = Image.FromFile("White.jpg"); // Đường dẫn ảnh
            e.Graphics.DrawImage(bg, new Rectangle(0, 0, txtLog.Width, txtLog.Height));
        }

        private void lblClients_Click(object sender, EventArgs e)
        {

        }
    }
}