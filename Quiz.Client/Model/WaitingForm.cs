using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quiz.Client.Model
{
    public partial class WaitingForm : Form
    {
        public WaitingForm()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile("Wait.jpg");
            this.BackgroundImageLayout = ImageLayout.Stretch;

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void WaitingForm_Load(object sender, EventArgs e)
        {

        }
    }
}
