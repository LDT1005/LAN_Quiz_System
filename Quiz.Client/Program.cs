using System;
using System.Windows.Forms;

namespace Quiz.Client
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (LoginForm login = new LoginForm())
            {
                // Chỉ chạy Dashboard khi Login trả về DialogResult.OK
                if (login.ShowDialog() == DialogResult.OK)
                {
                    ClientConnection conn = (ClientConnection)login.Tag;
                    Application.Run(new DashboardForm(conn));
                }
            }
        }
    }
}