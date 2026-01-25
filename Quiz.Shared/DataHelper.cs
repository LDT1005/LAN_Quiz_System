using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Quiz.Shared
{
    public static class DataHelper
    {
        public static void SendObject(NetworkStream stream, object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            byte[] data = Encoding.UTF8.GetBytes(json);
            byte[] lengthHeader = BitConverter.GetBytes(data.Length);
            byte[] fullPacket = new byte[4 + data.Length];
            Array.Copy(lengthHeader, 0, fullPacket, 0, 4);
            Array.Copy(data, 0, fullPacket, 4, data.Length);
            stream.Write(fullPacket, 0, fullPacket.Length);
            stream.Flush();
        }

        public static byte[] ReadPacket(NetworkStream stream)
        {
            try
            {
                byte[] lengthBuffer = new byte[4];
                int bytesRead = 0;
                while (bytesRead < 4)
                {
                    int read = stream.Read(lengthBuffer, bytesRead, 4 - bytesRead);
                    if (read <= 0) return null;
                    bytesRead += read;
                }
                int payloadLength = BitConverter.ToInt32(lengthBuffer, 0);
                byte[] payload = new byte[payloadLength];
                int payloadBytesRead = 0;
                while (payloadBytesRead < payloadLength)
                {
                    int read = stream.Read(payload, payloadBytesRead, payloadLength - payloadBytesRead);
                    if (read <= 0) return null;
                    payloadBytesRead += read;
                }
                return payload;
            }
            catch { return null; }
        }

        public static byte[] XorProcess(byte[] data, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] result = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                result[i] = (byte)(data[i] ^ keyBytes[i % keyBytes.Length]);
            return result;
        }

        public static string CreateChecksum(string data)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
                return Convert.ToBase64String(hash);
            }
        }
    }
}