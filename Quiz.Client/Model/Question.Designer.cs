namespace Quiz.Client.Model
{
    partial class Question
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblQuestionText = new System.Windows.Forms.Label();
            this.pnlOptionA = new System.Windows.Forms.Panel();
            this.pnlOptionC = new System.Windows.Forms.Panel();
            this.pnlOptionB = new System.Windows.Forms.Panel();
            this.pnlOptionD = new System.Windows.Forms.Panel();
            this.rbA = new System.Windows.Forms.RadioButton();
            this.rbC = new System.Windows.Forms.RadioButton();
            this.rbB = new System.Windows.Forms.RadioButton();
            this.rbD = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // lblQuestionText
            // 
            this.lblQuestionText.AutoSize = true;
            this.lblQuestionText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblQuestionText.Location = new System.Drawing.Point(12, 23);
            this.lblQuestionText.Name = "lblQuestionText";
            this.lblQuestionText.Size = new System.Drawing.Size(76, 20);
            this.lblQuestionText.TabIndex = 0;
            this.lblQuestionText.Text = "Câu hỏi :";
            // 
            // pnlOptionA
            // 
            this.pnlOptionA.Location = new System.Drawing.Point(102, 97);
            this.pnlOptionA.Name = "pnlOptionA";
            this.pnlOptionA.Size = new System.Drawing.Size(200, 100);
            this.pnlOptionA.TabIndex = 1;
            // 
            // pnlOptionC
            // 
            this.pnlOptionC.Location = new System.Drawing.Point(517, 97);
            this.pnlOptionC.Name = "pnlOptionC";
            this.pnlOptionC.Size = new System.Drawing.Size(200, 100);
            this.pnlOptionC.TabIndex = 2;
            this.pnlOptionC.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // pnlOptionB
            // 
            this.pnlOptionB.Location = new System.Drawing.Point(102, 262);
            this.pnlOptionB.Name = "pnlOptionB";
            this.pnlOptionB.Size = new System.Drawing.Size(200, 100);
            this.pnlOptionB.TabIndex = 3;
            // 
            // pnlOptionD
            // 
            this.pnlOptionD.Location = new System.Drawing.Point(517, 262);
            this.pnlOptionD.Name = "pnlOptionD";
            this.pnlOptionD.Size = new System.Drawing.Size(200, 100);
            this.pnlOptionD.TabIndex = 4;
            // 
            // rbA
            // 
            this.rbA.AutoSize = true;
            this.rbA.Location = new System.Drawing.Point(42, 97);
            this.rbA.Name = "rbA";
            this.rbA.Size = new System.Drawing.Size(37, 20);
            this.rbA.TabIndex = 5;
            this.rbA.TabStop = true;
            this.rbA.Text = "A";
            this.rbA.UseVisualStyleBackColor = true;
            this.rbA.Click += new System.EventHandler(this.rbA_Click);
            // 
            // rbC
            // 
            this.rbC.AutoSize = true;
            this.rbC.Location = new System.Drawing.Point(392, 97);
            this.rbC.Name = "rbC";
            this.rbC.Size = new System.Drawing.Size(37, 20);
            this.rbC.TabIndex = 6;
            this.rbC.TabStop = true;
            this.rbC.Text = "C";
            this.rbC.UseVisualStyleBackColor = true;
            this.rbC.CheckedChanged += new System.EventHandler(this.rbC_CheckedChanged);
            this.rbC.Click += new System.EventHandler(this.rbC_Click);
            // 
            // rbB
            // 
            this.rbB.AutoSize = true;
            this.rbB.Location = new System.Drawing.Point(42, 262);
            this.rbB.Name = "rbB";
            this.rbB.Size = new System.Drawing.Size(37, 20);
            this.rbB.TabIndex = 7;
            this.rbB.TabStop = true;
            this.rbB.Text = "B";
            this.rbB.UseVisualStyleBackColor = true;
            this.rbB.Click += new System.EventHandler(this.rbB_Click);
            // 
            // rbD
            // 
            this.rbD.AutoSize = true;
            this.rbD.Location = new System.Drawing.Point(392, 262);
            this.rbD.Name = "rbD";
            this.rbD.Size = new System.Drawing.Size(38, 20);
            this.rbD.TabIndex = 8;
            this.rbD.TabStop = true;
            this.rbD.Text = "D";
            this.rbD.UseVisualStyleBackColor = true;
            this.rbD.Click += new System.EventHandler(this.rbD_Click);
            // 
            // Question
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rbD);
            this.Controls.Add(this.rbB);
            this.Controls.Add(this.rbC);
            this.Controls.Add(this.rbA);
            this.Controls.Add(this.pnlOptionD);
            this.Controls.Add(this.pnlOptionB);
            this.Controls.Add(this.pnlOptionC);
            this.Controls.Add(this.pnlOptionA);
            this.Controls.Add(this.lblQuestionText);
            this.Name = "Question";
            this.Text = "Question";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblQuestionText;
        private System.Windows.Forms.Panel pnlOptionA;
        private System.Windows.Forms.Panel pnlOptionC;
        private System.Windows.Forms.Panel pnlOptionB;
        private System.Windows.Forms.Panel pnlOptionD;
        private System.Windows.Forms.RadioButton rbA;
        private System.Windows.Forms.RadioButton rbC;
        private System.Windows.Forms.RadioButton rbB;
        private System.Windows.Forms.RadioButton rbD;
    }
}