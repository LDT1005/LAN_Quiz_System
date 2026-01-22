using Quiz.Shared;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Quiz.Client
{
    public partial class ExamFormV2 : Form
    {
        private ClientConnection _conn;
        private ExamSession _session;
        private Dictionary<string, int> _userAnswers = new Dictionary<string, int>();
        private int _timeLeft;

        public ExamFormV2(ClientConnection conn, ExamSession session)
        {
            InitializeComponent();
            _conn = conn; _session = session;
            _timeLeft = session.DurationMinutes * 60;
            lblTitle.Text = session.ExamTitle;
            LoadQuestion(0);
            timer1.Start();
        }

        private void LoadQuestion(int index)
        {
            // Logic hiển thị câu hỏi lên UI (lblContent, rbA, rbB...)
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var submit = new AnswerSubmit { StudentID = "SV001", Answers = _userAnswers, SubmitTime = DateTime.Now };
            _conn.SendPacket(new Packet(Packet.TYPE_SUBMIT, "SV001", submit));
            MessageBox.Show("Nộp bài thành công!");
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _timeLeft--;
            lblTimer.Text = $"Còn lại: {_timeLeft / 60}:{_timeLeft % 60:D2}";
            if (_timeLeft <= 0) btnSubmit_Click(null, null);
        }
    }
}