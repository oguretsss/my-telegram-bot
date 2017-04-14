using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MyTelegramBot
{
  public class WeatherForecast
  {
    string url = ConfigurationManager.AppSettings["weatherApiRequestUrl"];
    private HttpClient weatherClient;

    public WeatherForecast()
    {
    }

    public int GetWeather()
    {
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
      //request.AutomaticDecompression = DecompressionMethods.GZip;
      string json = string.Empty;
      using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
      using (Stream stream = response.GetResponseStream())
      using (StreamReader reader = new StreamReader(stream))
      {
        json = reader.ReadToEnd();
      }

      JObject instafeed = JObject.Parse(json);
      JArray images = (JArray)instafeed["items"];
      List<string> imageList = images
                          .Select(img => (string)img["images"]["standard_resolution"]["url"])
                          .Take(quantity)
                          .ToList();
      return imageList;

      return 1;
    }
  }
}
