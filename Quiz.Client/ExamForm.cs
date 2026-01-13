using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz.Client
{
    public partial class ExamForm : Form
    {
        public ExamForm()
        {
            InitializeComponent();
        }

        private void lblNetStatus_Click(object sender, EventArgs e)
        {

        }

        private void ExamForm_Load(object sender, EventArgs e)
        {

        }
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            // Xử lý khi người dùng nhấn nút Nộp bài
            MessageBox.Show("Bạn đã nộp bài thành công!");
        }

    }
}
