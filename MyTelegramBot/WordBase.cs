using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MyTelegramBot
{
class WordBase
  {
    string path = @"..\..\resources\wordbase.json";

    Random rand = new Random();

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
      string exactPath = Path.GetFullPath(path);
      Console.WriteLine("Full path is " + exactPath);
      using (StreamReader str = new StreamReader(exactPath))
      {
          json = str.ReadToEnd();
          Dictionary<string, List<string>> values = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
          Greetings = values["greetings"];
          SingleAdjectives = values["single-adjectives"];
          ChatAdjectives = values["chat-adjectives"];
          SingleNouns = values["single-nouns"];
          ChatNouns = values["chat-nouns"];
      }
      Console.WriteLine(GenerateGreetingMessage(false));

    }

    public string GenerateGreetingMessage(bool chat = false)
    {
      string msg;
      int greetRand = rand.Next(Greetings.Count - 1);
      int adjRand = chat ? rand.Next(ChatAdjectives.Count - 1) : rand.Next(SingleAdjectives.Count - 1);
      int nounRand = chat ? rand.Next(ChatNouns.Count - 1) : rand.Next(SingleNouns.Count - 1);

      string greet = Greetings[greetRand];
      string adj = chat ? ChatAdjectives[adjRand] : SingleAdjectives[adjRand];
      string noun = chat ? ChatNouns[nounRand] : SingleNouns[nounRand];
      msg = greet + ", " + adj + " " + noun + "!"; 
      return msg;
    }
  }
}
