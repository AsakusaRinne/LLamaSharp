using LLama.Common;
using LLama.Examples.Extensions;
using System.Text;

namespace LLama.Examples.NewVersion
{
    public class StatelessModeExecute
    {
        private static string Utf16ToUtf8(string input)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(input);
            var convertedBytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, bytes);
            return Encoding.UTF8.GetString(convertedBytes);
        }
        public static async Task Run()
        {
            Console.Write("Please input your model path: ");
            var modelPath = @"D:\development\llama\weights\skywork-13b-base.Q4_K_M.gguf";

            var parameters = new ModelParams(modelPath)
            {
                ContextSize = 1024,
                Seed = 1337,
                GpuLayerCount = 15, 
            };
            using var model = LLamaWeights.LoadFromFile(parameters);
            var ex = new StatelessExecutor(model, parameters);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("The executor has been enabled. In this example, the inference is an one-time job. That says, the previous input and response has " +
                "no impact on the current response. Now you can ask it questions. Note that in this example, no prompt was set for LLM and the maximum response tokens is 50. " +
                "It may not perform well because of lack of prompt. This is also an example that could indicate the improtance of prompt in LLM. To improve it, you can add " +
                "a prompt for it yourself!");
            Console.ForegroundColor = ConsoleColor.White;

            var inferenceParams = new InferenceParams() { Temperature = 0.6f, AntiPrompts = new List<string> { "Question:", "#", "Question: ", ".\n" }, MaxTokens = 100 };

            while (true)
            {
                //Console.Write("\nQuestion: ");
                //Console.ForegroundColor = ConsoleColor.Green;
                //var prompt = Console.ReadLine();
                //Console.ForegroundColor = ConsoleColor.White;
                //Console.Write("Answer: ");
                string prompt = Utf16ToUtf8("从前有个山，山里有个庙，");
                await foreach (var text in ex.InferAsync(prompt, inferenceParams).Spinner())
                {
                    Console.Write(text);
                }
            }
        }
    }
}
