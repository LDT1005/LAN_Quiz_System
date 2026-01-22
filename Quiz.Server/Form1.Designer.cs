namespace Quiz.Server
{
    partial class Form1
    {
        private void InitializeComponent()
        {
            this.lblClients = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // lblClients
            this.lblClients.AutoSize = true;
            this.lblClients.Location = new System.Drawing.Point(12, 9);
            this.lblClients.Text = "Clients: 0";
            // txtLog
            this.txtLog.Location = new System.Drawing.Point(12, 40);
            this.txtLog.Multiline = true;
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(776, 398);
            // Form1
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblClients);
            this.Text = "Quiz Server";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        private System.Windows.Forms.Label lblClients;
        private System.Windows.Forms.TextBox txtLog;
    }
}