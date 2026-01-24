namespace Quiz.Client.Model
{
    partial class DashBoardForm
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
            this.lblWelcome = new System.Windows.Forms.Label();
            this.lblSystemStatus = new System.Windows.Forms.Label();
            this.btnReady = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblWelcome
            // 
            this.lblWelcome.AutoSize = true;
            this.lblWelcome.BackColor = System.Drawing.Color.Transparent;
            this.lblWelcome.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWelcome.ForeColor = System.Drawing.Color.Black;
            this.lblWelcome.Location = new System.Drawing.Point(269, 114);
            this.lblWelcome.Name = "lblWelcome";
            this.lblWelcome.Size = new System.Drawing.Size(201, 23);
            this.lblWelcome.TabIndex = 0;
            this.lblWelcome.Text = "Xin chào học sinh UTH";
            // 
            // lblSystemStatus
            // 
            this.lblSystemStatus.AutoSize = true;
            this.lblSystemStatus.ForeColor = System.Drawing.Color.Green;
            this.lblSystemStatus.Location = new System.Drawing.Point(307, 156);
            this.lblSystemStatus.Name = "lblSystemStatus";
            this.lblSystemStatus.Size = new System.Drawing.Size(109, 16);
            this.lblSystemStatus.TabIndex = 1;
            this.lblSystemStatus.Text = "Trạng thái kết nối";
            // 
            // btnReady
            // 
            this.btnReady.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btnReady.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnReady.Font = new System.Drawing.Font("Times New Roman", 10.2F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReady.Location = new System.Drawing.Point(300, 191);
            this.btnReady.Name = "btnReady";
            this.btnReady.Size = new System.Drawing.Size(136, 31);
            this.btnReady.TabIndex = 2;
            this.btnReady.Text = "Sẵn Sàng";
            this.btnReady.UseVisualStyleBackColor = false;
            // 
            // DashBoardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnReady);
            this.Controls.Add(this.lblSystemStatus);
            this.Controls.Add(this.lblWelcome);
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "DashBoardForm";
            this.Text = "DashBoardForm";
            this.Load += new System.EventHandler(this.DashBoardForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblWelcome;
        private System.Windows.Forms.Label lblSystemStatus;
        private System.Windows.Forms.Button btnReady;
    }
}