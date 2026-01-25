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
            try
            {
                string json = JsonConvert.SerializeObject(obj);
                byte[] data = Encoding.UTF8.GetBytes(json);
                byte[] lengthHeader = BitConverter.GetBytes(data.Length);
                byte[] fullPacket = new byte[4 + data.Length];
                Buffer.BlockCopy(lengthHeader, 0, fullPacket, 0, 4);
                Buffer.BlockCopy(data, 0, fullPacket, 4, data.Length);
                stream.Write(fullPacket, 0, fullPacket.Length);
                stream.Flush();
            }
            catch { /* Xử lý ngắt kết nối tại đây */ }
        }

        public static byte[] ReadPacket(NetworkStream stream)
        {
            try
            {
                byte[] lengthBuffer = new byte[4];
                if (!ReadExact(stream, lengthBuffer, 4)) return null;

                int payloadLength = BitConverter.ToInt32(lengthBuffer, 0);
                byte[] payload = new byte[payloadLength];
                if (!ReadExact(stream, payload, payloadLength)) return null;

                return payload;
            }
            catch { return null; }
        }

        private static bool ReadExact(NetworkStream stream, byte[] buffer, int length)
        {
            int totalRead = 0;
            while (totalRead < length)
            {
                int read = stream.Read(buffer, totalRead, length - totalRead);
                if (read <= 0) return false;
                totalRead += read;
            }
            return true;
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