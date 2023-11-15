using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LLama.Examples
{
    internal class Utils
    {
        private static string BytesToString(byte[] bytes)
        {
            string myString;
            Encoding fromEcoding = Encoding.GetEncoding("gb2312");
            Encoding toEcoding = Encoding.GetEncoding("UTF-8");
            byte[] toBytes = Encoding.Convert(fromEcoding, toEcoding, bytes);
            myString = toEcoding.GetString(toBytes);//将字节数组解码成字符串
            return myString;
        }
    }
}
