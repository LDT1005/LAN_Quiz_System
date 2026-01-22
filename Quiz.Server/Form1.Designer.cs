namespace Quiz.Server
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblClients = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblClients
            // 
            this.lblClients.AutoSize = true;
            this.lblClients.Location = new System.Drawing.Point(12, 9);
            this.lblClients.Name = "lblClients";
            this.lblClients.Size = new System.Drawing.Size(60, 16);
            this.lblClients.TabIndex = 0;
            this.lblClients.Text = "Clients: 0";
            // ❌ ĐÃ XÓA DÒNG Click EVENT
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblClients);
            this.Name = "Form1";
            this.Text = "Quiz Server";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblClients;
    }
}
