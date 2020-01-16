using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GraphDrawer
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    public static class BackendWordProcessing
    {
        private static Dictionary<string, int> WordCounter(string textFile, Dictionary<string, int> wordCounter)
        {
            Regex symbols = new Regex("[^a-zA-Z ]");
            string textFileClean = symbols.Replace(textFile, string.Empty);
            string textFileCleanUpper = textFileClean.ToUpper();
            string[] words = textFileCleanUpper.Split();

            for (int i = 0; i < words.Length; i++)
            {
                if (words[i] != "")
                {
                    if (wordCounter.ContainsKey(words[i]))
                    {
                        wordCounter[words[i]] = wordCounter[words[i]] + 1;
                    }
                    else
                    {
                        wordCounter.Add($"{words[i]}", 1);
                    }
                    Console.WriteLine($"{words[i]} = {wordCounter[words[i]]}");
                }
            }

            return wordCounter;
        }

        private static List<KeyValuePair<string, int>> GraphSetup(Dictionary<string, int> xyValues)
        {
            var xyValuesList = xyValues.ToList();
            xyValuesList.Sort((word1, word2) => word2.Value.CompareTo(word1.Value));
            return xyValuesList;
        }

        public static List<KeyValuePair<string, int>> Process(string fileInput)
        {
            Dictionary<string, int> words = new Dictionary<string, int>();
            if (fileInput != "")
            {
                string fileText = File.ReadAllText(@$"{fileInput}");
                words = WordCounter(fileText, words);
                return GraphSetup(words);
            }
            words = WordCounter("InvalidFile", words);
            return GraphSetup(words);


        }
    }
}
