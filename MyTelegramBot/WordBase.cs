using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MyTelegramBot
{
    class WordBase
    {
        string path = @"D:\VSProjects\MyBot\my-telegram-bot\MyTelegramBot\resources\wordbase.json";

        public List<string> Greetings { get; set; }
        public List<string> SingleAdjectives { get; set; }
        public List<string> ChatAdjectives { get; set; }
        public List<string> SingleNouns { get; set; }
        public List<string> ChatNouns { get; set; }


        public WordBase()
                {
                    Init();
                }


        private void Init()
        {
            string json;
            using (StreamReader str = new StreamReader(path))
            {
                json = str.ReadToEnd();
                Dictionary<string, List<string>> values = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
                Greetings = values["greetings"];
                SingleAdjectives = values["single-adjectives"];
                ChatAdjectives = values["single-adjectives"];
                SingleNouns = values["single-nouns"];
                ChatNouns = values["chat-nouns"];
            }
            Console.WriteLine(GenerateGreetingMessage(false));

        }

        public string GenerateGreetingMessage(bool chat = false)
        {
            string msg;
            Random rand = new Random();
            int greetRand = rand.Next(Greetings.Count - 1);
            int adjRand = chat ? rand.Next(ChatAdjectives.Count - 1) : rand.Next(SingleAdjectives.Count - 1);
            int nounRand = chat ? rand.Next(ChatNouns.Count - 1) : rand.Next(SingleNouns.Count - 1);

            string greet = Greetings[greetRand];
            string adj = chat ? ChatAdjectives[adjRand] : SingleAdjectives[adjRand];
            string noun = chat ? ChatNouns[adjRand] : SingleNouns[adjRand];
            msg = greet + ", " + adj + " " + noun + "!"; 
            return msg;
        }
    }
}
