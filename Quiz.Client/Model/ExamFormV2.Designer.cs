namespace Quiz.Client.Model
{
    partial class ExamFormV2
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTimer = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.pnlAnswers = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlQuestionGrid = new System.Windows.Forms.FlowLayoutPanel();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Size = new System.Drawing.Size(120, 21);
            this.lblTitle.Text = "Đang tải đề...";

            // lblTimer
            this.lblTimer.AutoSize = true;
            this.lblTimer.ForeColor = System.Drawing.Color.Red;
            this.lblTimer.Location = new System.Drawing.Point(12, 45);
            this.lblTimer.Text = "Thời gian: 00:00";

            // btnBack
            this.btnBack.Location = new System.Drawing.Point(15, 380);
            this.btnBack.Size = new System.Drawing.Size(75, 30);
            this.btnBack.Text = "< Trước";
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);

            // btnNext
            this.btnNext.Location = new System.Drawing.Point(100, 380);
            this.btnNext.Size = new System.Drawing.Size(75, 30);
            this.btnNext.Text = "Tiếp >";
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);

            // btnSubmit (button3)
            this.btnSubmit.BackColor = System.Drawing.Color.Orange;
            this.btnSubmit.Location = new System.Drawing.Point(680, 380);
            this.btnSubmit.Size = new System.Drawing.Size(100, 35);
            this.btnSubmit.Text = "NỘP BÀI";
            this.btnSubmit.Click += new System.EventHandler(this.button3_Click);

            // timer1
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);

            // FlowPanels
            this.pnlAnswers.Location = new System.Drawing.Point(15, 80);
            this.pnlAnswers.Size = new System.Drawing.Size(550, 280);
            this.pnlQuestionGrid.Location = new System.Drawing.Point(580, 80);
            this.pnlQuestionGrid.Size = new System.Drawing.Size(200, 280);

            // Form
            this.ClientSize = new System.Drawing.Size(800, 430);
            this.Controls.AddRange(new System.Windows.Forms.Control[] { this.btnSubmit, this.btnNext, this.btnBack, this.pnlQuestionGrid, this.pnlAnswers, this.progressBar1, this.lblTimer, this.lblTitle });
            this.Name = "ExamFormV2";
            this.Text = "Classroom Quiz - Exam";
            this.Load += new System.EventHandler(this.ExamFormV2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblTimer;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.FlowLayoutPanel pnlAnswers;
        private System.Windows.Forms.FlowLayoutPanel pnlQuestionGrid;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Timer timer1;
    }
}