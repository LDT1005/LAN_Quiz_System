using Newtonsoft.Json;
using Quiz.Server.Network;
using Quiz.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Quiz.Server
{
    public partial class Form1 : Form
    {
        private ServerManager _server = new ServerManager();
        private ExamSession _currentExam;

        public Form1()
        {
            InitializeComponent();

            // Đăng ký các sự kiện từ ServerManager
            _server.OnClientCountChanged += c => Invoke(new Action(() => lblClients.Text = "Clients: " + c));
            _server.OnAnswerReceived += s => Invoke(new Action(() => {
                txtLog.AppendText($"[{DateTime.Now:HH:mm}] SV {s.StudentID} nộp bài thành công.\r\n");
            }));
            _server.OnLog += msg => Invoke(new Action(() => txtLog.AppendText($"[{DateTime.Now:HH:mm}] {msg}\r\n")));

            try
            {
                picLogo.Image = Image.FromFile("logo.png");
                picLogo.SizeMode = PictureBoxSizeMode.Zoom;
                this.BackgroundImage = Image.FromFile("White.jpg");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch { }
        }

        private void Form1_Load(object sender, EventArgs e) => _server.Start(8888);

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
            else MessageBox.Show("Vui lòng Import đề trước!");
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog { Filter = "CSV|*.csv", FileName = "KetQua.csv" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    var results = _server.GetFinalResults();
                    StringBuilder sb = new StringBuilder("MSSV,Diem\n");
                    foreach (var r in results) sb.AppendLine($"{r.StudentID},{r.TotalScore}");
                    System.IO.File.WriteAllText(sfd.FileName, sb.ToString(), Encoding.UTF8);
                    MessageBox.Show("Đã xuất báo cáo!");
                }
            }
        }

        private void btnSendNotice_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNotice.Text))
            {
                _server.SendBroadcast(txtNotice.Text, "HIGH");
                txtNotice.Clear();
            }
        }

        // CÁC PHƯƠNG THỨC FIX LỖI CS1061 (DESIGNER STUBS)
        private void lblClients_Click(object sender, EventArgs e) { }
        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }
        private void txtNotice_TextChanged(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void textBox1_TextChanged(object sender, EventArgs e) { }
    }
}