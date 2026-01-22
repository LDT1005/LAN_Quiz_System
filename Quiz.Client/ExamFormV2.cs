using System;
using System.Windows.Forms;
using Quiz.Client.Network;

namespace Quiz.Client
{
    public partial class ExamFormV2 : Form
    {
        private ExamClientService _service;

        public ExamFormV2(ClientConnection connection)
        {
            InitializeComponent();
            _service = new ExamClientService(connection);
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // Giả sử mã SV001 và câu Q001 cho demo
            _service.SubmitAnswer("SV001", "Q001", 1);
            MessageBox.Show("Đã nộp bài!");
        }
    }
}