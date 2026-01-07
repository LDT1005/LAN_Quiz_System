using Newtonsoft.Json;
using System;
using System.Text;

namespace Quiz.Shared
{
    public static class DataHelper
    {
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

        // Chuyển chuỗi JSON ngược lại thành Object
        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}