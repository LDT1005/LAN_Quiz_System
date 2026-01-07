using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace Quiz.Shared
{
    public static class DataHelper
    {
        // ========== SERIALIZE/DESERIALIZE ==========

        // Chuyển Object thành mảng Byte có kèm 4 byte độ dài ở đầu
        public static byte[] Serialize(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            byte[] data = Encoding.UTF8.GetBytes(json);
            byte[] sizePrefix = BitConverter.GetBytes(data.Length); // 4 byte chứa độ dài
            byte[] fullPacket = new byte[4 + data.Length];
            Array.Copy(sizePrefix, 0, fullPacket, 0, 4);
            Array.Copy(data, 0, fullPacket, 4, data.Length);
            return fullPacket;
        }

        // Deserialize từ chuỗi JSON
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }

        // Deserialize từ mảng byte (bỏ qua 4 byte header)
        public static T Deserialize<T>(byte[] data)
        {
            // Nếu data có 4 byte header, bỏ qua nó
            int offset = 0;
            if (data.Length > 4)
            {
                int declaredLength = BitConverter.ToInt32(data, 0);
                if (declaredLength == data.Length - 4)
                {
                    offset = 4; // Có header, bỏ qua
                }
            }

            string json = Encoding.UTF8.GetString(data, offset, data.Length - offset);
            return JsonConvert.DeserializeObject<T>(json);
        }

        // ========== NETWORK STREAM OPERATIONS ==========

        // Đọc packet từ NetworkStream với TCP Framing
        public static byte[] ReadPacket(NetworkStream stream, int timeoutSeconds = 30)
        {
            stream.ReadTimeout = timeoutSeconds * 1000;

            // Đọc 4 byte header (độ dài payload)
            byte[] lengthBuffer = new byte[4];
            int bytesRead = 0;

            while (bytesRead < 4)
            {
                int read = stream.Read(lengthBuffer, bytesRead, 4 - bytesRead);
                if (read == 0)
                    throw new IOException("Connection closed while reading header");
                bytesRead += read;
            }

            int payloadLength = BitConverter.ToInt32(lengthBuffer, 0);

            // Validate độ dài hợp lệ (tránh tấn công)
            if (payloadLength <= 0 || payloadLength > 10 * 1024 * 1024) // Max 10MB
                throw new InvalidDataException($"Invalid payload length: {payloadLength}");

            // Đọc payload
            byte[] payload = new byte[payloadLength];
            bytesRead = 0;

            while (bytesRead < payloadLength)
            {
                int read = stream.Read(payload, bytesRead, payloadLength - bytesRead);
                if (read == 0)
                    throw new IOException("Connection closed while reading payload");
                bytesRead += read;
            }

            return payload;
        }

        // Gửi packet qua NetworkStream
        public static void SendPacket(NetworkStream stream, byte[] data)
        {
            stream.Write(data, 0, data.Length);
            stream.Flush();
        }

        // Gửi object trực tiếp
        public static void SendObject(NetworkStream stream, object obj)
        {
            byte[] data = Serialize(obj);
            SendPacket(stream, data);
        }

        // ========== XOR ENCRYPTION (Đơn giản) ==========

        // Mã hóa XOR với key
        public static byte[] XorEncrypt(byte[] data, string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] result = new byte[data.Length];

            for (int i = 0; i < data.Length; i++)
            {
                result[i] = (byte)(data[i] ^ keyBytes[i % keyBytes.Length]);
            }

            return result;
        }

        // Giải mã XOR (giống mã hóa)
        public static byte[] XorDecrypt(byte[] data, string key)
        {
            return XorEncrypt(data, key); // XOR là symmetric
        }

        // Mã hóa chuỗi JSON
        public static string EncryptJson(object obj, string key)
        {
            string json = JsonConvert.SerializeObject(obj);
            byte[] data = Encoding.UTF8.GetBytes(json);
            byte[] encrypted = XorEncrypt(data, key);
            return Convert.ToBase64String(encrypted);
        }

        // Giải mã chuỗi JSON
        public static T DecryptJson<T>(string encryptedBase64, string key)
        {
            byte[] encrypted = Convert.FromBase64String(encryptedBase64);
            byte[] decrypted = XorDecrypt(encrypted, key);
            string json = Encoding.UTF8.GetString(decrypted);
            return JsonConvert.DeserializeObject<T>(json);
        }

        // ========== HELPER METHODS ==========

        // Tạo checksum đơn giản
        public static string CreateChecksum(string data)
        {
            int hash = 0;
            foreach (char c in data)
            {
                hash = ((hash << 5) - hash) + c;
            }
            return hash.ToString("X8");
        }

        // Validate checksum
        public static bool ValidateChecksum(string data, string checksum)
        {
            return CreateChecksum(data) == checksum;
        }
    }
}