using LLama.Common;
using System.Text;

namespace LLama.Examples.Examples
{
    public class ChatSessionWithRoleName
    {
        private static string GB2312ToUtf8(string input)
        {
            var gb2312 = Encoding.GetEncoding("gb2312");
            byte[] bytes = gb2312.GetBytes(input);
            var convertedBytes = Encoding.Convert(gb2312, Encoding.UTF8, bytes);
            return Encoding.UTF8.GetString(convertedBytes);
        }
        private static string Utf8ToGB2312(string input)
        {
            var gb2312 = Encoding.GetEncoding("gb2312");
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            var convertedBytes = Encoding.Convert(Encoding.UTF8, gb2312, bytes);
            return gb2312.GetString(convertedBytes);
        }

        private static string ConvertFromEncodingToAnother(string input, Encoding original, Encoding target)
        {
            byte[] bytes = original.GetBytes(input);
            var convertedBytes = Encoding.Convert(original, target, bytes);
            return target.GetString(convertedBytes);
        }

        public static async Task Run()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.Write("Please input your model path: ");
            var modelPath = "D:\\development\\llama\\weights\\Baichuan2-7B-Chat\\ggml-model-f16-q5_0.gguf";
            var prompt = File.ReadAllText("Assets/chat-with-kunkun-chinese.txt", encoding: Encoding.GetEncoding("gb2312")).Trim();
            prompt = ConvertFromEncodingToAnother(prompt, Encoding.GetEncoding("gb2312"), Encoding.UTF8);

            var parameters = new ModelParams(modelPath)
            {
                ContextSize = 1024,
                Seed = 1337,
                GpuLayerCount = 20,
                Encoding = Encoding.UTF8
            };
            using var model = LLamaWeights.LoadFromFile(parameters);
            using var context = model.CreateContext(parameters);
            var executor = new InteractiveExecutor(context);

            var session = new ChatSession(executor).WithHistoryTransform(new LLamaTransforms.DefaultHistoryTransform("用户"));

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("The chat session has started. In this example, the prompt is printed for better visual result.");
            Console.ForegroundColor = ConsoleColor.White;

            // show the prompt
            Console.Write(prompt);
            while (true)
            {
                await foreach (var text in session.ChatAsync(prompt, new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "用户：" } }))
                {
                    //Console.Write(text);
                    Console.Write(ConvertFromEncodingToAnother(text, Encoding.UTF8, Encoding.GetEncoding("gb2312")));
                }

                Console.ForegroundColor = ConsoleColor.Green;
                prompt = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
