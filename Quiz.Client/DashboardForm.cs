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
    public partial class DashboardForm : Form
    {
        private NetworkClientService _client;

        public DashboardForm(NetworkClientService client)
        {
            InitializeComponent();
            _client = client;
        }

        private void btnStartExam_Click(object sender, EventArgs e)
        {

        }

        private void btnRequestExam_Click(object sender, EventArgs e)
        {

        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {

        }
    }
}
