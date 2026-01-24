using Quiz.Shared;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quiz.Client.Model
{
    public partial class ResultForm : Form
    {
        private ExamResult _result;
        private ExamSession _exam;

        // THÊM CONSTRUCTOR NÀY - QUAN TRỌNG
        public ResultForm(ExamResult result, ExamSession exam)
        {
            InitializeComponent();
            _result = result;
            _exam = exam;
        }

        // Constructor mặc định (cho Designer)
        public ResultForm()
        {
            InitializeComponent();
        }

        private void ResultForm_Load(object sender, EventArgs e)
        {
            if (_result == null) return;

            // Hiển thị điểm số
            lblScore.Text = $"{_result.TotalScore:F1} / {_result.MaxScore:F1}";

            // Tính phần trăm
            double percentage = (_result.TotalScore / _result.MaxScore) * 100;
            lblCorrectCount.Text = $"Số câu đúng: {_result.CorrectCount}/{_result.TotalQuestions}";
            lblAccuracy.Text = $"Độ chính xác: {percentage:F2}%";

            // Đổi màu dựa trên điểm
            if (percentage >= 80)
            {
                lblScore.ForeColor = Color.Green;
            }
            else if (percentage >= 50)
            {
                lblScore.ForeColor = Color.Orange;
            }
            else
            {
                lblScore.ForeColor = Color.Red;
            }
        }

        private void lblScore_Click(object sender, EventArgs e)
        {
            // Giữ nguyên event handler
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            if (_result == null || _exam == null)
            {
                MessageBox.Show("Không có dữ liệu chi tiết!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Tạo form chi tiết
            var detailsForm = new Form
            {
                Text = "Chi Tiết Kết Quả",
                Width = 700,
                Height = 500,
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };

            // Tạo ListView
            var listView = new ListView
            {
                Dock = DockStyle.Fill,
                View = View.Details,
                FullRowSelect = true,
                GridLines = true
            };

            // Thêm cột
            listView.Columns.Add("Câu", 50);
            listView.Columns.Add("Nội dung", 250);
            listView.Columns.Add("Đáp án của bạn", 120);
            listView.Columns.Add("Đáp án đúng", 120);
            listView.Columns.Add("Kết quả", 60);

            // Thêm dữ liệu
            for (int i = 0; i < _result.Details.Count; i++)
            {
                var detail = _result.Details[i];
                var question = _exam.Questions[i];

                string yourAnswer = "Không làm";
                if (detail.SelectedAnswer >= 0 && detail.SelectedAnswer < question.Options.Count)
                {
                    yourAnswer = $"{(char)('A' + detail.SelectedAnswer)}. {question.Options[detail.SelectedAnswer]}";
                }

                string correctAnswer = "N/A";
                if (detail.CorrectAnswer >= 0 && detail.CorrectAnswer < question.Options.Count)
                {
                    correctAnswer = $"{(char)('A' + detail.CorrectAnswer)}. {question.Options[detail.CorrectAnswer]}";
                }

                var item = new ListViewItem(new[]
                {
                    (i + 1).ToString(),
                    question.Content.Length > 60 ? question.Content.Substring(0, 57) + "..." : question.Content,
                    yourAnswer.Length > 30 ? yourAnswer.Substring(0, 27) + "..." : yourAnswer,
                    correctAnswer.Length > 30 ? correctAnswer.Substring(0, 27) + "..." : correctAnswer,
                    detail.IsCorrect ? "✓ Đúng" : "✗ Sai"
                });

                // Đổi màu hàng
                item.BackColor = detail.IsCorrect ? Color.LightGreen : Color.LightPink;
                listView.Items.Add(item);
            }

            // Thêm ListView vào form
            detailsForm.Controls.Add(listView);

            // Hiển thị form
            detailsForm.ShowDialog();
        }
    }
}