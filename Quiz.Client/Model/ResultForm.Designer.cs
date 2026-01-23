namespace Quiz.Client.Model
{
    partial class ResultForm
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
            this.lblScore = new System.Windows.Forms.Label();
            this.lblCorrectCount = new System.Windows.Forms.Label();
            this.lblAccuracy = new System.Windows.Forms.Label();
            this.btnDetails = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScore.Location = new System.Drawing.Point(325, 95);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(115, 39);
            this.lblScore.TabIndex = 0;
            this.lblScore.Text = "label1";
            this.lblScore.Click += new System.EventHandler(this.lblScore_Click);
            // 
            // lblCorrectCount
            // 
            this.lblCorrectCount.AutoSize = true;
            this.lblCorrectCount.Location = new System.Drawing.Point(329, 176);
            this.lblCorrectCount.Name = "lblCorrectCount";
            this.lblCorrectCount.Size = new System.Drawing.Size(45, 16);
            this.lblCorrectCount.TabIndex = 1;
            this.lblCorrectCount.Text = "17 / 20";
            // 
            // lblAccuracy
            // 
            this.lblAccuracy.AutoSize = true;
            this.lblAccuracy.Location = new System.Drawing.Point(407, 176);
            this.lblAccuracy.Name = "lblAccuracy";
            this.lblAccuracy.Size = new System.Drawing.Size(33, 16);
            this.lblAccuracy.TabIndex = 2;
            this.lblAccuracy.Text = "85%";
            // 
            // btnDetails
            // 
            this.btnDetails.Location = new System.Drawing.Point(332, 218);
            this.btnDetails.Name = "btnDetails";
            this.btnDetails.Size = new System.Drawing.Size(107, 23);
            this.btnDetails.TabIndex = 3;
            this.btnDetails.Text = "Xem Chi Tiết";
            this.btnDetails.UseVisualStyleBackColor = true;
            this.btnDetails.Click += new System.EventHandler(this.btnDetails_Click);
            // 
            // ResultForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnDetails);
            this.Controls.Add(this.lblAccuracy);
            this.Controls.Add(this.lblCorrectCount);
            this.Controls.Add(this.lblScore);
            this.Name = "ResultForm";
            this.Text = "ResultForm";
            this.Load += new System.EventHandler(this.ResultForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label lblCorrectCount;
        private System.Windows.Forms.Label lblAccuracy;
        private System.Windows.Forms.Button btnDetails;
    }
}