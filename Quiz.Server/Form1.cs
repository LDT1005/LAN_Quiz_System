using Quiz.Server.Network;
using Quiz.Shared;
using System;
using System.Windows.Forms;

namespace Quiz.Server
{
    public partial class Form1 : Form
    {
        private ServerManager _server;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _server = new ServerManager();
            _server.OnClientCountChanged += count => {
                this.Invoke(new Action(() => lblClients.Text = "Clients: " + count));
            };

            _server.OnAnswerReceived += submit => {
                this.Invoke(new Action(() => {
                    string info = string.Format("[{0}] BÀI NỘP | SV: {1} | Câu: {2} | Đ/A: {3}{4}",
                        DateTime.Now.ToString("HH:mm:ss"), submit.StudentID, submit.QuestionID, submit.SelectedAnswer, Environment.NewLine);
                    txtLog.AppendText(info);
                    txtLog.ScrollToCaret();
                }));
            };
            _server.Start(8888);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) => _server?.Stop();
    }
}