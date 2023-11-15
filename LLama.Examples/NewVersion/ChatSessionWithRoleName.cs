using LLama.Common;
using System.Text;

namespace LLama.Examples.NewVersion
{
    public class ChatSessionWithRoleName
    {
        public static async Task Run()
        {
            Console.Write("Please input your model path: ");
            var modelPath = @"D:\development\llama\weights\thespis-13b-v0.5.Q8_0.gguf";
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var encoding = Encoding.GetEncoding("gb2312");
            var prompt = File.ReadAllText("Assets/chat-with-kunkun-chinese.txt", encoding: encoding).Trim();

            var parameters = new ModelParams(modelPath)
            {
                ContextSize = 1024,
                Seed = 1337,
                GpuLayerCount = 15, 
                Encoding = encoding,
            };
            using var model = LLamaWeights.LoadFromFile(parameters);
            using var context = model.CreateContext(parameters);
            var executor = new InteractiveExecutor(context);

            var session = new ChatSession(executor);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("The chat session has started. In this example, the prompt is printed for better visual result.");
            Console.ForegroundColor = ConsoleColor.White;

            // show the prompt
            Console.Write(prompt);
            while (true)
            {
                await foreach (var text in session.ChatAsync(prompt, new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "User:" } }))
                {
                    Console.Write(text);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                prompt = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}
