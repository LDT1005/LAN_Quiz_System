using Newtonsoft.Json;
using Quiz.Server.Network;
using Quiz.Shared;
using System;
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
            _server.OnClientCountChanged += c => Invoke(new Action(() => lblClients.Text = $"Clients: {c}"));
            _server.OnAnswerReceived += s => Invoke(new Action(() => {
                txtLog.AppendText($"[{DateTime.Now:HH:mm}] SV {s.StudentID} nộp bài thành công.\r\n");
            }));
            _server.OnLog += msg => Invoke(new Action(() => txtLog.AppendText($"[{DateTime.Now:HH:mm}] {msg}\r\n")));

            // Giải quyết triệt để lỗi liệt nút bấm bằng Z-Order
            btnOpenPort.BringToFront();
            btnImportExam.BringToFront();
            btnStartExam.BringToFront();
            btnExport.BringToFront();
            btnSendNotice.BringToFront();
        }

        private void btnOpenPort_Click(object sender, EventArgs e)
        {
            try
            {
                _server.Start(8888);
                btnOpenPort.Text = "Cổng Đã Mở";
                btnOpenPort.BackColor = Color.LightGreen;
                btnOpenPort.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnImportExam_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog { Filter = "JSON|*.json" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    _currentExam = JsonConvert.DeserializeObject<ExamSession>(System.IO.File.ReadAllText(ofd.FileName));
                    txtLog.AppendText($"[HỆ THỐNG]: Đã nhận đề thi '{_currentExam.ExamTitle}'\r\n");
                }
            }
        }

        private void btnStartExam_Click_1(object sender, EventArgs e)
        {
            if (_currentExam != null)
            {
                _server.BroadcastExam(_currentExam);
                txtLog.AppendText("[HỆ THỐNG]: Đã phát đề thi tới thí sinh.\r\n");
            }
            else MessageBox.Show("Vui lòng Import đề trước!");
        }

        private void btnExport_Click_1(object sender, EventArgs e)
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

        private void btnSendNotice_Click_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtNotice.Text))
            {
                _server.SendBroadcast(txtNotice.Text, "HIGH");
                txtLog.AppendText($"[THÔNG BÁO]: {txtNotice.Text}\r\n");
                txtNotice.Clear();
            }
        }

        // Các stub tránh lỗi Designer
        private void lblClients_Click(object sender, EventArgs e) { }
        private void txtLog_TextChanged(object sender, EventArgs e)
        {
            txtLog.SelectionStart = txtLog.Text.Length;
            txtLog.ScrollToCaret();
        }
        private void txtNotice_TextChanged(object sender, EventArgs e) { }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
    }
}