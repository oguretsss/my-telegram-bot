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
    string url = @"http://api.openweathermap.org/data/2.5/weather?id=524901&appid=e702171c9cac4d1eebacff92c8571b65";

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

      JObject forecast = JObject.Parse(json);
      int temperature = forecast["main"]["temp"].Value<int>();
      temperature -= 273;

      return temperature;
    }
  }
}
