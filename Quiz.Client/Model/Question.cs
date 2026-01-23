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
    public partial class Question : Form
    {
        public Question()
        {
           
        }

        private void InitializeComponent()
        {
            this.lblQuestionText = new System.Windows.Forms.Label();
            this.pnlOptionA = new System.Windows.Forms.Panel();
            this.pnlOptionB = new System.Windows.Forms.Panel();
            this.pnlOptionC = new System.Windows.Forms.Panel();
            this.pnlOptionD = new System.Windows.Forms.Panel();
            this.rbA = new System.Windows.Forms.RadioButton();
            this.rbC = new System.Windows.Forms.RadioButton();
            this.rbB = new System.Windows.Forms.RadioButton();
            this.rbD = new System.Windows.Forms.RadioButton();
            this.lblAnswerA = new System.Windows.Forms.Label();
            this.lblAnswerB = new System.Windows.Forms.Label();
            this.lblAnswerC = new System.Windows.Forms.Label();
            this.lblAnswerD = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblQuestionText
            // 
            this.lblQuestionText.AutoSize = true;
            this.lblQuestionText.Location = new System.Drawing.Point(1, 20);
            this.lblQuestionText.Name = "lblQuestionText";
            this.lblQuestionText.Size = new System.Drawing.Size(44, 16);
            this.lblQuestionText.TabIndex = 0;
            this.lblQuestionText.Text = "label1";
            // 
            // pnlOptionA
            // 
            this.pnlOptionA.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pnlOptionA.Location = new System.Drawing.Point(4, 71);
            this.pnlOptionA.Name = "pnlOptionA";
            this.pnlOptionA.Size = new System.Drawing.Size(77, 20);
            this.pnlOptionA.TabIndex = 2;
            // 
            // pnlOptionB
            // 
            this.pnlOptionB.Location = new System.Drawing.Point(4, 107);
            this.pnlOptionB.Name = "pnlOptionB";
            this.pnlOptionB.Size = new System.Drawing.Size(77, 19);
            this.pnlOptionB.TabIndex = 0;
            this.pnlOptionB.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // pnlOptionC
            // 
            this.pnlOptionC.Location = new System.Drawing.Point(4, 141);
            this.pnlOptionC.Name = "pnlOptionC";
            this.pnlOptionC.Size = new System.Drawing.Size(77, 20);
            this.pnlOptionC.TabIndex = 3;
            // 
            // pnlOptionD
            // 
            this.pnlOptionD.Location = new System.Drawing.Point(4, 179);
            this.pnlOptionD.Name = "pnlOptionD";
            this.pnlOptionD.Size = new System.Drawing.Size(76, 20);
            this.pnlOptionD.TabIndex = 4;
            // 
            // rbA
            // 
            this.rbA.AutoSize = true;
            this.rbA.Location = new System.Drawing.Point(88, 71);
            this.rbA.Name = "rbA";
            this.rbA.Size = new System.Drawing.Size(37, 20);
            this.rbA.TabIndex = 0;
            this.rbA.TabStop = true;
            this.rbA.Text = "A";
            this.rbA.UseVisualStyleBackColor = true;
            // 
            // rbC
            // 
            this.rbC.AutoSize = true;
            this.rbC.Location = new System.Drawing.Point(87, 141);
            this.rbC.Name = "rbC";
            this.rbC.Size = new System.Drawing.Size(37, 20);
            this.rbC.TabIndex = 1;
            this.rbC.TabStop = true;
            this.rbC.Text = "C";
            this.rbC.UseVisualStyleBackColor = true;
            // 
            // rbB
            // 
            this.rbB.AutoSize = true;
            this.rbB.Location = new System.Drawing.Point(87, 107);
            this.rbB.Name = "rbB";
            this.rbB.Size = new System.Drawing.Size(37, 20);
            this.rbB.TabIndex = 2;
            this.rbB.TabStop = true;
            this.rbB.Text = "B";
            this.rbB.UseVisualStyleBackColor = true;
            // 
            // rbD
            // 
            this.rbD.AutoSize = true;
            this.rbD.Location = new System.Drawing.Point(87, 179);
            this.rbD.Name = "rbD";
            this.rbD.Size = new System.Drawing.Size(38, 20);
            this.rbD.TabIndex = 3;
            this.rbD.TabStop = true;
            this.rbD.Text = "D";
            this.rbD.UseVisualStyleBackColor = true;
            // 
            // lblAnswerA
            // 
            this.lblAnswerA.AutoSize = true;
            this.lblAnswerA.Location = new System.Drawing.Point(150, 65);
            this.lblAnswerA.Name = "lblAnswerA";
            this.lblAnswerA.Padding = new System.Windows.Forms.Padding(5);
            this.lblAnswerA.Size = new System.Drawing.Size(54, 26);
            this.lblAnswerA.TabIndex = 5;
            this.lblAnswerA.Text = "label3";
            // 
            // lblAnswerB
            // 
            this.lblAnswerB.AutoSize = true;
            this.lblAnswerB.Location = new System.Drawing.Point(150, 100);
            this.lblAnswerB.Name = "lblAnswerB";
            this.lblAnswerB.Padding = new System.Windows.Forms.Padding(5);
            this.lblAnswerB.Size = new System.Drawing.Size(54, 26);
            this.lblAnswerB.TabIndex = 6;
            this.lblAnswerB.Text = "label4";
            // 
            // lblAnswerC
            // 
            this.lblAnswerC.AutoSize = true;
            this.lblAnswerC.Location = new System.Drawing.Point(150, 135);
            this.lblAnswerC.Name = "lblAnswerC";
            this.lblAnswerC.Padding = new System.Windows.Forms.Padding(5);
            this.lblAnswerC.Size = new System.Drawing.Size(54, 26);
            this.lblAnswerC.TabIndex = 7;
            this.lblAnswerC.Text = "label5";
            // 
            // lblAnswerD
            // 
            this.lblAnswerD.AutoSize = true;
            this.lblAnswerD.Location = new System.Drawing.Point(150, 173);
            this.lblAnswerD.Name = "lblAnswerD";
            this.lblAnswerD.Padding = new System.Windows.Forms.Padding(5);
            this.lblAnswerD.Size = new System.Drawing.Size(54, 26);
            this.lblAnswerD.TabIndex = 8;
            this.lblAnswerD.Text = "label6";
            // 
            // Question
            // 
            this.ClientSize = new System.Drawing.Size(792, 429);
            this.Controls.Add(this.lblAnswerD);
            this.Controls.Add(this.lblAnswerC);
            this.Controls.Add(this.lblAnswerB);
            this.Controls.Add(this.lblAnswerA);
            this.Controls.Add(this.rbA);
            this.Controls.Add(this.rbC);
            this.Controls.Add(this.rbB);
            this.Controls.Add(this.rbD);
            this.Controls.Add(this.pnlOptionD);
            this.Controls.Add(this.pnlOptionC);
            this.Controls.Add(this.pnlOptionB);
            this.Controls.Add(this.pnlOptionA);
            this.Controls.Add(this.lblQuestionText);
            this.Name = "Question";
            this.Load += new System.EventHandler(this.Question_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Question_Load(object sender, EventArgs e)
        {

        }
    }
}
