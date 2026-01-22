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
            Load += Form1_Load;
            FormClosing += Form1_FormClosing;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _server = new ServerManager();

            _server.OnClientCountChanged += count =>
            {
                Invoke(new Action(() =>
                {
                    lblClients.Text = "Clients: " + count;
                }));
            };

            _server.OnAnswerReceived += submit =>
            {
                Invoke(new Action(() =>
                {
                    Console.WriteLine(
                        "BÀI NỘP | SV: " + submit.StudentID +
                        " | Câu: " + submit.QuestionID +
                        " | Đáp án: " + submit.SelectedAnswer
                    );
                }));
            };

            _server.Start(8888);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _server.Stop();
        }
    }
}
