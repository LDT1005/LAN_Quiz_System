using Newtonsoft.Json;
using Quiz.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz.Client.Model
{
    public partial class ExamFormV2 : Form
    {
        private ClientConnection _connection;
        private ExamSession _exam;
        private Dictionary<string, int> _answers = new Dictionary<string, int>();
        private int _currentQuestionIndex = 0;
        private DateTime _endTime;
        private List<RadioButton> _radioButtons = new List<RadioButton>();

        public ExamFormV2(ClientConnection connection, ExamSession exam)
        {
            InitializeComponent();
            _connection = connection;
            _exam = exam;
            _endTime = DateTime.Now.AddMinutes(_exam.DurationMinutes);
        }

        public ExamFormV2() { InitializeComponent(); }

        private void ExamFormV2_Load(object sender, EventArgs e)
        {
            if (_exam == null) return;
            lblTitle.Text = _exam.ExamTitle;

            // Khôi phục bài làm từ file tạm nếu có sự cố
            string tempPath = GetTempFilePath();
            if (File.Exists(tempPath))
            {
                try
                {
                    string savedJson = File.ReadAllText(tempPath);
                    _answers = JsonConvert.DeserializeObject<Dictionary<string, int>>(savedJson);
                }
                catch { InitializeEmptyAnswers(); }
            }
            else { InitializeEmptyAnswers(); }

            timer1.Interval = 1000;
            timer1.Start();
            SetupQuestionGrid();
            LoadQuestion(0);
        }

        private void InitializeEmptyAnswers()
        {
            foreach (var q in _exam.Questions) { _answers[q.ID] = -1; }
        }

        private string GetTempFilePath() => Path.Combine(Application.StartupPath, $"temp_{_connection.StudentID}.json");

        private void SaveProgressLocal()
        {
            try { File.WriteAllText(GetTempFilePath(), JsonConvert.SerializeObject(_answers)); } catch { }
        }

        private void SetupQuestionGrid()
        {
            pnlQuestionGrid.Controls.Clear();
            for (int i = 0; i < _exam.Questions.Count; i++)
            {
                var btn = new Button { Text = (i + 1).ToString(), Width = 35, Height = 35, Tag = i, BackColor = Color.LightGray };
                btn.Click += (s, ev) => LoadQuestion((int)((Button)s).Tag);
                pnlQuestionGrid.Controls.Add(btn);
            }
        }

        private void LoadQuestion(int index)
        {
            if (index < 0 || index >= _exam.Questions.Count) return;
            _currentQuestionIndex = index;
            var q = _exam.Questions[index];
            pnlAnswers.Controls.Clear();
            _radioButtons.Clear();

            var lbl = new Label { Text = $"Câu {index + 1}: {q.Content}", AutoSize = true, Font = new Font(this.Font, FontStyle.Bold), MaximumSize = new Size(pnlAnswers.Width - 20, 0) };
            pnlAnswers.Controls.Add(lbl);

            for (int i = 0; i < q.Options.Count; i++)
            {
                var rb = new RadioButton { Text = $"{(char)('A' + i)}. {q.Options[i]}", AutoSize = true, Tag = i };
                rb.CheckedChanged += (s, ev) => {
                    if (((RadioButton)s).Checked)
                    {
                        _answers[q.ID] = (int)((RadioButton)s).Tag;
                        UpdateQuestionGridColors();
                        SaveProgressLocal();
                    }
                };
                if (_answers[q.ID] == i) rb.Checked = true;
                _radioButtons.Add(rb);
                pnlAnswers.Controls.Add(rb);
            }
            UpdateQuestionGridColors();
            btnBack.Enabled = index > 0;
            btnNext.Enabled = index < _exam.Questions.Count - 1;
        }

        private void UpdateQuestionGridColors()
        {
            for (int i = 0; i < pnlQuestionGrid.Controls.Count; i++)
            {
                var btn = (Button)pnlQuestionGrid.Controls[i];
                string qID = _exam.Questions[i].ID;
                if (i == _currentQuestionIndex) btn.BackColor = Color.LightBlue;
                else btn.BackColor = (_answers[qID] >= 0) ? Color.LightGreen : Color.LightGray;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var rem = _endTime - DateTime.Now;
            if (rem.TotalSeconds <= 0) { timer1.Stop(); SubmitExam(true); return; }
            lblTimer.Text = $"Thời gian: {rem.Minutes:D2}:{rem.Seconds:D2}";
            progressBar1.Value = (int)Math.Max(0, (rem.TotalSeconds / (_exam.DurationMinutes * 60)) * 100);
        }

        // FIX LỖI CS1061: Phương thức xử lý sự kiện nút Gửi
        private void button3_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc chắn muốn nộp bài?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                SubmitExam(false);
            }
        }

        private void btnBack_Click(object sender, EventArgs e) => LoadQuestion(_currentQuestionIndex - 1);
        private void btnNext_Click(object sender, EventArgs e) => LoadQuestion(_currentQuestionIndex + 1);

        private void SubmitExam(bool auto)
        {
            timer1.Stop();
            btnSubmit.Enabled = false;
            try
            {
                string json = JsonConvert.SerializeObject(_answers);
                var sub = new AnswerSubmit
                {
                    StudentID = _connection.StudentID,
                    EncryptedData = Convert.ToBase64String(DataHelper.XorProcess(Encoding.UTF8.GetBytes(json), _connection.StudentID)),
                    Checksum = DataHelper.CreateChecksum(json),
                    IsAutoSubmit = auto
                };
                DataHelper.SendObject(_connection.Stream, new Packet(Packet.TYPE_SUBMIT, _connection.StudentID, sub));
                WaitForResult();
            }
            catch (Exception ex) { MessageBox.Show("Lỗi nộp bài: " + ex.Message); btnSubmit.Enabled = true; }
        }

        private async void WaitForResult()
        {
            byte[] p = await Task.Run(() => DataHelper.ReadPacket(_connection.Stream));
            if (p != null)
            {
                var pack = JsonConvert.DeserializeObject<Packet>(Encoding.UTF8.GetString(p));
                if (pack.Type == Packet.TYPE_RESULT)
                {
                    if (File.Exists(GetTempFilePath())) File.Delete(GetTempFilePath());
                    var res = JsonConvert.DeserializeObject<ExamResult>(pack.Data.ToString());
                    this.Invoke(new Action(() => { new ResultForm(res, _exam).ShowDialog(); this.Close(); }));
                }
            }
        }
    }
}