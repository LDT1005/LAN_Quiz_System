namespace Quiz.Client
{
    partial class DashboardForm
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
            this.txtHost = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblExamTitle = new System.Windows.Forms.Label();
            this.lblDuration = new System.Windows.Forms.Label();
            this.btnRequestExam = new System.Windows.Forms.Button();
            this.btnStartExam = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtHost
            // 
            this.txtHost.Location = new System.Drawing.Point(0, 2);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(100, 22);
            this.txtHost.TabIndex = 0;
            this.txtHost.Text = "192. 168.44.1";
            // 
            // txtPort
            // 
            this.txtPort.Location = new System.Drawing.Point(106, 2);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(62, 22);
            this.txtPort.TabIndex = 1;
            this.txtPort.Text = "8888";
            this.txtPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(0, 50);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(143, 22);
            this.txtUser.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(-3, 87);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(67, 16);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Trạng thái";
            // 
            // lblExamTitle
            // 
            this.lblExamTitle.AutoSize = true;
            this.lblExamTitle.Location = new System.Drawing.Point(12, 103);
            this.lblExamTitle.Name = "lblExamTitle";
            this.lblExamTitle.Size = new System.Drawing.Size(41, 16);
            this.lblExamTitle.TabIndex = 4;
            this.lblExamTitle.Text = "Kỳ thi:";
            // 
            // lblDuration
            // 
            this.lblDuration.AutoSize = true;
            this.lblDuration.Location = new System.Drawing.Point(-3, 119);
            this.lblDuration.Name = "lblDuration";
            this.lblDuration.Size = new System.Drawing.Size(73, 16);
            this.lblDuration.TabIndex = 5;
            this.lblDuration.Text = "Thời lượng:";
            // 
            // btnRequestExam
            // 
            this.btnRequestExam.Location = new System.Drawing.Point(0, 148);
            this.btnRequestExam.Name = "btnRequestExam";
            this.btnRequestExam.Size = new System.Drawing.Size(104, 23);
            this.btnRequestExam.TabIndex = 6;
            this.btnRequestExam.Text = "Yêu cầu đề thi";
            this.btnRequestExam.UseVisualStyleBackColor = true;
            this.btnRequestExam.Click += new System.EventHandler(this.btnRequestExam_Click);
            // 
            // btnStartExam
            // 
            this.btnStartExam.Location = new System.Drawing.Point(110, 148);
            this.btnStartExam.Name = "btnStartExam";
            this.btnStartExam.Size = new System.Drawing.Size(93, 23);
            this.btnStartExam.TabIndex = 7;
            this.btnStartExam.Text = "Bắt đầu thi";
            this.btnStartExam.UseVisualStyleBackColor = true;
            this.btnStartExam.Click += new System.EventHandler(this.btnStartExam_Click);
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnStartExam);
            this.Controls.Add(this.btnRequestExam);
            this.Controls.Add(this.lblDuration);
            this.Controls.Add(this.lblExamTitle);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.txtUser);
            this.Controls.Add(this.txtPort);
            this.Controls.Add(this.txtHost);
            this.Name = "DashboardForm";
            this.Text = "DashboardForm";
            this.Load += new System.EventHandler(this.DashboardForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label lblExamTitle;
        private System.Windows.Forms.Label lblDuration;
        private System.Windows.Forms.Button btnRequestExam;
        private System.Windows.Forms.Button btnStartExam;
        private System.Windows.Forms.TextBox txtPort;
    }
}