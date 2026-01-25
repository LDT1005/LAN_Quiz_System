using System;
using System.Drawing;
using System.Windows.Forms;

namespace Quiz.Client.Model
{
    public partial class WaitingForm : Form
    {
        public WaitingForm(string studentName)
        {
            InitializeComponent();
            try
            {
                this.BackgroundImage = Image.FromFile("Wait.jpg");
                this.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch { }

            // Hiển thị tên học sinh lên giao diện
            lblStudentNameInfo.Text = $"Học sinh: {studentName}";
        }

        // Giữ lại constructor mặc định cho Designer
        public WaitingForm() { InitializeComponent(); }

        private void WaitingForm_Load(object sender, EventArgs e) { }
        private void label2_Click(object sender, EventArgs e) { }
    }
}