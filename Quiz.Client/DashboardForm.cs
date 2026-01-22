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

        // Fix CS1061: Bổ sung phương thức Load
        private void DashboardForm_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Đã kết nối Server";
        }

        // Fix CS1061: Bổ sung phương thức Click
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

        private void btnStartExam_Click(object sender, EventArgs e)
        {
            ExamFormV2 exam = new ExamFormV2(_connection); // Fix CS7036
            exam.Show();
            this.Hide();
        }
    }
}