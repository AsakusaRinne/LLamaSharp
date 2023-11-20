using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LLama
{
    internal class CustomizedTokenizer
    {
        static Dictionary<int, string> idToString;
        static Dictionary<string, int> stringToId;
        static CustomizedTokenizer()
        {
            idToString = new Dictionary<int, string>();
            stringToId = new Dictionary<string, int>(StringComparer.Ordinal);

            // 读取文件内容
            using (StreamReader reader = new StreamReader(@"D:\development\llama\weights\vocab.txt"))
            {
                string line;
                int id = 0;

                while ((line = reader.ReadLine()) != null)
                {
                    // 将每一行内容添加到字典中
                    if(id != 6)
                    {
                        idToString[id] = line;
                        stringToId[line] = id;
                    }
                    id++;
                }
            }
            stringToId["\n"] = 5;
            //stringToId["\r\n"] = 6;
            idToString[5] = "\n";
            //idToString[6] = "\r\n";
        }

        public static int[] Tokenize(string input)
        {
            List<int> numbers = new List<int>();
            int index = 0;

            while (index < input.Length)
            {
                string substring = string.Empty;
                int id = -1;
                int pre_idx = -1;

                // 从当前位置开始依次向后截取子串，并检查是否在字典中
                for (int i = index; i < input.Length; i++)
                {
                    substring += input[i].ToString();

                    if (stringToId.ContainsKey(substring))
                    {
                        id = stringToId[substring];
                        pre_idx = i;
                    }
                }
                if (id == -1)
                {
                    throw new DirectoryNotFoundException();
                }
                numbers.Add(id);
                index = pre_idx + 1;
                if(numbers.Count == 31)
                {
                    Console.WriteLine();
                }
            }

            return numbers.ToArray();
        }

        public static string DeTokenize(int id)
        {
            return idToString[id];
        }
    }
}
