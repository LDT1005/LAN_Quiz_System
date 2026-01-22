using System;
using System.Windows.Forms;

namespace Quiz.Client
{
    public partial class DashboardForm : Form
    {
        private ClientConnection _connection;

        public DashboardForm(ClientConnection connection)
        {
            InitializeComponent();
            _connection = connection;
        }

        // FIX LỖI CS1061: Phương thức Designer đang tìm kiếm
        private void DashboardForm_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Đã kết nối Server";
        }

        private void btnRequestExam_Click(object sender, EventArgs e)
        {
            try
            {
                _connection.SendString("REQUEST_EXAM");
                string res = _connection.ReceiveString();
                lblExamTitle.Text = "Kỳ thi: " + res;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        // FIX LỖI CS1061 và CS7036:
        private void btnStartExam_Click(object sender, EventArgs e)
        {
            // Truyền connection vào constructor của ExamFormV2
            ExamFormV2 exam = new ExamFormV2(_connection);
            exam.Show();
            this.Hide();
        }
    }
}