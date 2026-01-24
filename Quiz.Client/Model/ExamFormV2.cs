using Newtonsoft.Json;
using Quiz.Shared;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
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

        // THÊM CONSTRUCTOR NÀY - QUAN TRỌNG
        public ExamFormV2(ClientConnection connection, ExamSession exam)
        {
            InitializeComponent();
            _connection = connection;
            _exam = exam;
            _endTime = _exam.StartTime.AddMinutes(_exam.DurationMinutes);
        }

        // Constructor mặc định (cho Designer)
        public ExamFormV2()
        {
            InitializeComponent();
        }

        private void ExamFormV2_Load(object sender, EventArgs e)
        {
            if (_exam == null) return;

            lblTitle.Text = _exam.ExamTitle;

            // Initialize answers dictionary
            foreach (var q in _exam.Questions)
            {
                _answers[q.ID] = -1; // -1 = not answered
            }

            // Setup timer
            timer1.Interval = 1000; // 1 second
            timer1.Tick += Timer1_Tick;
            timer1.Start();

            // Setup question grid
            SetupQuestionGrid();

            // Load first question
            LoadQuestion(0);

            UpdateNavigationButtons();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            var remaining = _endTime - DateTime.Now;

            if (remaining.TotalSeconds <= 0)
            {
                timer1.Stop();
                MessageBox.Show("Hết giờ! Bài thi sẽ được nộp tự động.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                SubmitExam(true);
                return;
            }

            lblTimer.Text = $"Thời gian còn lại: {remaining.Minutes:D2}:{remaining.Seconds:D2}";

            // Update progress bar
            double totalSeconds = _exam.DurationMinutes * 60;
            double remainingSeconds = remaining.TotalSeconds;
            progressBar1.Value = (int)((remainingSeconds / totalSeconds) * 100);

            // Warning when < 5 minutes
            if (remaining.TotalMinutes < 5)
            {
                lblTimer.ForeColor = Color.Red;
            }
        }

        private void SetupQuestionGrid()
        {
            pnlQuestionGrid.Controls.Clear();
            pnlQuestionGrid.FlowDirection = FlowDirection.LeftToRight;
            pnlQuestionGrid.WrapContents = true;
            pnlQuestionGrid.AutoScroll = true;

            for (int i = 0; i < _exam.Questions.Count; i++)
            {
                var btn = new Button
                {
                    Text = (i + 1).ToString(),
                    Width = 40,
                    Height = 40,
                    Tag = i,
                    BackColor = Color.LightGray
                };

                btn.Click += QuestionGridButton_Click;
                pnlQuestionGrid.Controls.Add(btn);
            }
        }

        private void QuestionGridButton_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            LoadQuestion((int)btn.Tag);
        }

        private void LoadQuestion(int index)
        {
            if (index < 0 || index >= _exam.Questions.Count) return;

            _currentQuestionIndex = index;
            var question = _exam.Questions[index];

            // Clear previous answers display
            pnlAnswers.Controls.Clear();
            _radioButtons.Clear();

            // Show question content
            var lblQuestion = new Label
            {
                Text = $"Câu {index + 1}: {question.Content}",
                AutoSize = true,
                Font = new Font(this.Font.FontFamily, 10, FontStyle.Bold),
                MaximumSize = new Size(pnlAnswers.Width - 20, 0),
                Padding = new Padding(5)
            };
            pnlAnswers.Controls.Add(lblQuestion);

            // Show options as radio buttons
            for (int i = 0; i < question.Options.Count; i++)
            {
                var rb = new RadioButton
                {
                    Text = $"{(char)('A' + i)}. {question.Options[i]}",
                    AutoSize = true,
                    MaximumSize = new Size(pnlAnswers.Width - 20, 0),
                    Padding = new Padding(5, 3, 5, 3),
                    Tag = i
                };

                rb.CheckedChanged += RadioButton_CheckedChanged;
                _radioButtons.Add(rb);
                pnlAnswers.Controls.Add(rb);

                // Restore previous answer if exists
                if (_answers[question.ID] == i)
                {
                    rb.Checked = true;
                }
            }

            UpdateQuestionGridColors();
            UpdateNavigationButtons();
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            var rb = (RadioButton)sender;
            if (rb.Checked)
            {
                var question = _exam.Questions[_currentQuestionIndex];
                _answers[question.ID] = (int)rb.Tag;
                UpdateQuestionGridColors();
            }
        }

        private void UpdateQuestionGridColors()
        {
            for (int i = 0; i < pnlQuestionGrid.Controls.Count; i++)
            {
                var btn = (Button)pnlQuestionGrid.Controls[i];
                var questionID = _exam.Questions[i].ID;

                if (_answers[questionID] >= 0)
                {
                    btn.BackColor = Color.LightGreen; // Answered
                }
                else
                {
                    btn.BackColor = Color.LightGray; // Not answered
                }

                if (i == _currentQuestionIndex)
                {
                    btn.BackColor = Color.LightBlue; // Current question
                }
            }
        }

        private void UpdateNavigationButtons()
        {
            btnBack.Enabled = _currentQuestionIndex > 0;
            btnNext.Enabled = _currentQuestionIndex < _exam.Questions.Count - 1;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (_currentQuestionIndex > 0)
            {
                LoadQuestion(_currentQuestionIndex - 1);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_currentQuestionIndex < _exam.Questions.Count - 1)
            {
                LoadQuestion(_currentQuestionIndex + 1);
            }
        }

        private void button3_Click(object sender, EventArgs e) // btnSubmit
        {
            int unansweredCount = _answers.Values.Count(x => x < 0);

            string message = unansweredCount > 0
                ? $"Bạn còn {unansweredCount} câu chưa làm. Bạn có chắc muốn nộp bài?"
                : "Bạn có chắc muốn nộp bài?";

            var result = MessageBox.Show(message, "Xác nhận nộp bài",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                SubmitExam(false);
            }
        }

        private void SubmitExam(bool isAutoSubmit)
        {
            timer1.Stop();
            btnSubmit.Enabled = false;
            btnBack.Enabled = false;
            btnNext.Enabled = false;

            try
            {
                var submit = new AnswerSubmit
                {
                    StudentID = _connection.StudentID,
                    Answers = new Dictionary<string, int>(_answers),
                    SubmitTime = DateTime.Now,
                    IsAutoSubmit = isAutoSubmit
                };

                var packet = new Packet(Packet.TYPE_SUBMIT, _connection.StudentID, submit);
                string json = JsonConvert.SerializeObject(packet) + "\n";
                byte[] dataBytes = Encoding.UTF8.GetBytes(json);

                _connection.Stream.Write(dataBytes, 0, dataBytes.Length);
                _connection.Stream.Flush();

                lblTimer.Text = "Đang chờ kết quả...";
                lblTimer.ForeColor = Color.Blue;

                // Wait for result
                WaitForResult();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nộp bài:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void WaitForResult()
        {
            try
            {
                byte[] buffer = new byte[8192];
                int bytesRead = await _connection.Stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead).Trim();

                var packet = JsonConvert.DeserializeObject<Packet>(response);

                if (packet.Type == Packet.TYPE_RESULT)
                {
                    var result = JsonConvert.DeserializeObject<ExamResult>(packet.Data.ToString());
                    ShowResult(result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi nhận kết quả:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void ShowResult(ExamResult result)
        {
            var resultForm = new ResultForm(result, _exam);
            this.Hide();
            resultForm.ShowDialog();
            this.Close();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (timer1.Enabled)
            {
                var result = MessageBox.Show("Bài thi chưa nộp. Bạn có chắc muốn thoát?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }

            timer1?.Stop();
            base.OnFormClosing(e);
        }
    }
}