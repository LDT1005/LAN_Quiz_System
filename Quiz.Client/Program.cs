using System;
using System.Windows.Forms;
using Quiz.Client.Model;

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
                if (login.ShowDialog() == DialogResult.OK)
                {
                    ClientConnection conn = login.Tag as ClientConnection;

                    // SỬA: Dùng DashBoardForm (chữ B hoa) theo tên file thật của bạn
                    Application.Run(new DashBoardForm(conn));
                }
            }
        }
    }
}