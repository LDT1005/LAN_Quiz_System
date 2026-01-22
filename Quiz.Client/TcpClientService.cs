using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;

namespace Quiz.Client
{
    public class TcpClientService
    {
        private const string SERVER_IP = "192.168.44.1";
        private const int SERVER_PORT = 8888;

        public void Send(object data)
        {
            using (TcpClient client = new TcpClient())
            {
                client.Connect(SERVER_IP, SERVER_PORT);

                NetworkStream stream = client.GetStream();

                string json = JsonConvert.SerializeObject(data);
                byte[] body = Encoding.UTF8.GetBytes(json);
                byte[] length = BitConverter.GetBytes(body.Length);

                stream.Write(length, 0, length.Length);
                stream.Write(body, 0, body.Length);
            }
        }
    }
}
