namespace Quiz.Server
{
    partial class Form1
    {
        private void InitializeComponent()
        {
            this.lblClients = new System.Windows.Forms.Label();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnImportExam = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnStartExam = new System.Windows.Forms.Button();
            this.btnSendNotice = new System.Windows.Forms.Button();
            this.txtNotice = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.lblClientCount = new System.Windows.Forms.Label();
            this.colMSSV = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHoTen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblClients
            // 
            this.lblClients.AutoSize = true;
            this.lblClients.Location = new System.Drawing.Point(12, 9);
            this.lblClients.Name = "lblClients";
            this.lblClients.Size = new System.Drawing.Size(60, 16);
            this.lblClients.TabIndex = 1;
            this.lblClients.Text = "Clients: 0";
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(12, 28);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(776, 398);
            this.txtLog.TabIndex = 0;
            this.txtLog.TextChanged += new System.EventHandler(this.txtLog_TextChanged);
            // 
            // btnImportExam
            // 
            this.btnImportExam.Location = new System.Drawing.Point(290, 119);
            this.btnImportExam.Name = "btnImportExam";
            this.btnImportExam.Size = new System.Drawing.Size(153, 23);
            this.btnImportExam.TabIndex = 2;
            this.btnImportExam.Text = "Import Đề Thi (JSON)";
            this.btnImportExam.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(151, 119);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(128, 23);
            this.btnExport.TabIndex = 3;
            this.btnExport.Text = "Xuất Báo Cáo (CSV)";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // btnStartExam
            // 
            this.btnStartExam.Location = new System.Drawing.Point(151, 173);
            this.btnStartExam.Name = "btnStartExam";
            this.btnStartExam.Size = new System.Drawing.Size(128, 23);
            this.btnStartExam.TabIndex = 4;
            this.btnStartExam.Text = "BẮT ĐẦU THI";
            this.btnStartExam.UseVisualStyleBackColor = true;
            // 
            // btnSendNotice
            // 
            this.btnSendNotice.Location = new System.Drawing.Point(290, 173);
            this.btnSendNotice.Name = "btnSendNotice";
            this.btnSendNotice.Size = new System.Drawing.Size(153, 23);
            this.btnSendNotice.TabIndex = 5;
            this.btnSendNotice.Text = "Gửi Thông Báo";
            this.btnSendNotice.UseVisualStyleBackColor = true;
            // 
            // txtNotice
            // 
            this.txtNotice.Location = new System.Drawing.Point(179, 214);
            this.txtNotice.Name = "txtNotice";
            this.txtNotice.Size = new System.Drawing.Size(189, 22);
            this.txtNotice.TabIndex = 6;
            this.txtNotice.TextChanged += new System.EventHandler(this.txtNotice_TextChanged);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMSSV,
            this.colHoTen,
            this.colStatus});
            this.dataGridView1.Location = new System.Drawing.Point(151, 242);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(428, 53);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // lblClientCount
            // 
            this.lblClientCount.AutoSize = true;
            this.lblClientCount.Location = new System.Drawing.Point(12, 31);
            this.lblClientCount.Name = "lblClientCount";
            this.lblClientCount.Size = new System.Drawing.Size(128, 16);
            this.lblClientCount.TabIndex = 8;
            this.lblClientCount.Text = "Clients Connected: 0";
            // 
            // colMSSV
            // 
            this.colMSSV.HeaderText = "MSSV";
            this.colMSSV.MinimumWidth = 6;
            this.colMSSV.Name = "colMSSV";
            this.colMSSV.Width = 125;
            // 
            // colHoTen
            // 
            this.colHoTen.HeaderText = "Họ và Tên";
            this.colHoTen.MinimumWidth = 6;
            this.colHoTen.Name = "colHoTen";
            this.colHoTen.Width = 125;
            // 
            // colStatus
            // 
            this.colStatus.HeaderText = "Trạng thái";
            this.colStatus.MinimumWidth = 6;
            this.colStatus.Name = "colStatus";
            this.colStatus.Width = 125;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblClientCount);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtNotice);
            this.Controls.Add(this.btnSendNotice);
            this.Controls.Add(this.btnStartExam);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnImportExam);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.lblClients);
            this.Name = "Form1";
            this.Text = "Quiz Server";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Label lblClients;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Button btnImportExam;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnStartExam;
        private System.Windows.Forms.Button btnSendNotice;
        private System.Windows.Forms.TextBox txtNotice;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label lblClientCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMSSV;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHoTen;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStatus;
    }
}