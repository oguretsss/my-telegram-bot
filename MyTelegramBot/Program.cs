using System;
using System.Configuration;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputMessageContents;
using Telegram.Bot.Types.ReplyMarkups;

namespace MyTelegramBot
{
  class Program
    {
      private static readonly TelegramBotClient Bot = new TelegramBotClient(ConfigurationManager.AppSettings["myToken"]);
      private static WordBase wb;
      private static WeatherForecast forecast;
    private static List<string> greetingStrings;
    static void Main(string[] args)
    {
      wb = new WordBase();
      forecast = new WeatherForecast();
      greetingStrings = new List<string>();
      greetingStrings.AddRange(new string[] { "привет", "шалом", "здорово,", "доброе утро"});
      Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnInlineQuery += BotOnInlineQueryReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            Bot.OnReceiveError += BotOnReceiveError;

            var me = Bot.GetMeAsync().Result;

            Console.Title = me.Username;

            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
      Debugger.Break();
        }

        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received choosen inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            InlineQueryResult[] results = {
                new InlineQueryResultLocation
                {
                    Id = "1",
                    Latitude = 40.7058316f, // displayed result
                    Longitude = -74.2581888f,
                    Title = "New York",
                    InputMessageContent = new InputLocationMessageContent // message if result is selected
                    {
                        Latitude = 40.7058316f,
                        Longitude = -74.2581888f,
                    }
                },

                new InlineQueryResultLocation
                {
                    Id = "2",
                    Longitude = 52.507629f, // displayed result
                    Latitude = 13.1449577f,
                    Title = "Berlin",
                    InputMessageContent = new InputLocationMessageContent // message if result is selected
                    {
                        Longitude = 52.507629f,
                        Latitude = 13.1449577f
                    }
                }
            };

            await Bot.AnswerInlineQueryAsync(inlineQueryEventArgs.InlineQuery.Id, results, isPersonal: true, cacheTime: 0);
        }

    private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
    {
      var message = messageEventArgs.Message;
      
      if (message == null || message.Type != MessageType.TextMessage) return;

      if ((message.Chat.Type == ChatType.Group || message.Chat.Type == ChatType.Supergroup) && IsGreetMessage(message.Text))
      {
        string greetingMessage = wb.GenerateGreetingMessage(true);

        await Bot.SendTextMessageAsync(message.Chat.Id, greetingMessage,
            replyMarkup: new ReplyKeyboardHide());
      }
      if ((message.Chat.Type == ChatType.Group || message.Chat.Type == ChatType.Supergroup) 
        && (message.Text.ToLower().Contains("федот") || message.Text.ToLower().Contains("манул")))
      {
        string msg = "Манул, кстати, еще тот говноед!";

        await Bot.SendTextMessageAsync(message.Chat.Id, msg,
            replyMarkup: new ReplyKeyboardHide());
      }

      else if((message.Chat.Type == ChatType.Group || message.Chat.Type == ChatType.Supergroup)
        && message.Text.ToLower().Contains("мартын"))
      {
        string msg = "Мартын, учи питон!";

        await Bot.SendTextMessageAsync(message.Chat.Id, msg,
            replyMarkup: new ReplyKeyboardHide());
      }
      else if (message.Text.StartsWith("/hello"))
      {
        string greetingMessage = wb.GenerateGreetingMessage(false);

        await Bot.SendTextMessageAsync(message.Chat.Id, greetingMessage,
                replyMarkup: new ReplyKeyboardHide());
      }
      else if (message.Text.ToLower().StartsWith("да, бот?"))
      {
        string msg;
        if (message.From.Id == 142654796)
        {
          msg = "Да, хозяин";
        }
        else
        {
          msg = "А жопе слова не давали!";
        }

        await Bot.SendTextMessageAsync(message.Chat.Id, msg,
              replyMarkup: new ReplyKeyboardHide());
      }
      else if ((message.Text.StartsWith("/weather")) || ((message.Chat.Type == ChatType.Group || message.Chat.Type == ChatType.Supergroup)
        && message.Text.ToLower().Contains("погода")))
      {
        int weather = forecast.GetWeather();
        string msg = "Погода в Москве сейчас " + weather + " градусов по Цельсию.";

        await Bot.SendTextMessageAsync(message.Chat.Id, msg,
              replyMarkup: new ReplyKeyboardHide());
      }
      else if (message.Text.StartsWith("/debug"))
      {
        string msg = "Debug info:\t\n";
        msg += "Message Date: "+message.Date.ToShortTimeString()+"\n";
        msg += "Sender ID: "+message.From.Id+"\n";
        msg += "Sender Last Name: "+message.From.LastName+"\n";
        msg += "Sender User Name: " + message.From.Username+"\n";
        //msg += "Reply To message text: " + message.ReplyToMessage.Text+"\n";

        await Bot.SendTextMessageAsync(message.Chat.Id, msg,
              replyMarkup: new ReplyKeyboardHide());
      }

      else if (message.Text.StartsWith("/party") && message.Chat.Type == ChatType.Private)
      {
        UserSession session  = SessionManager.GetSessionByUserId(message.From.Id);
        session.CurrentState = SessionState.CreateParty;
        string msg = @"Для создания новой вечеринки введите следущие данные: 
Имя организатора
Название мероприятия
Время начала (в любом формате)
Описание вечеринки

Внимание! Каждое поле должно начинаться с новой строки. Например:
Вася
Дико угарная вечеринка у Васи на хате.
Сегодня в 22.30
Всех жду, все угарим, приносите еду и выпивку с собой";

        await Bot.SendTextMessageAsync(message.Chat.Id, msg,
              replyMarkup: new ReplyKeyboardHide());
      }
      else if(message.Text.StartsWith("/help"))
      {
        var usage = @"Usage:
/hello   - bot will say hello to you creatively =)
";

        await Bot.SendTextMessageAsync(message.Chat.Id, usage,
                replyMarkup: new ReplyKeyboardHide());
      }
      else if(message.Text.StartsWith("/list"))
      {
        string msg = "";
        if(PartyManager.Parties == null)
        {
          msg += "No parties planned for today =(";
          await Bot.SendTextMessageAsync(message.Chat.Id, msg,
        replyMarkup: new ReplyKeyboardHide());
        }
        else
        {
          msg += "List of parties planned for today: \n";
          foreach (var item in PartyManager.Parties)
          {
            msg += item.PartyName + "\n";
          }
          await Bot.SendTextMessageAsync(message.Chat.Id, msg,
          replyMarkup: new ReplyKeyboardHide()); 
        }
      }
      else if(message.Chat.Type == ChatType.Private)
      {
        UserSession session = SessionManager.GetSessionByUserId(message.From.Id);
        if(session.CurrentState == SessionState.CreateParty)
        {
          string[] partyInfo = message.Text.Split('\n');
          foreach (var item in partyInfo)
          {
            Console.WriteLine(item);
          }
          if (partyInfo.Length != 4)
            await Bot.SendTextMessageAsync(message.Chat.Id, "Try again please", replyMarkup: new ReplyKeyboardHide());
          else
          {
            Party party = new Party(message.From.Id, partyInfo[0], partyInfo[1], partyInfo[2], partyInfo[3]);
            PartyManager.EditOrCreateParty(party);
            string msg = "Вечеринка успешно создана:\n";
            msg += "Организатор вечеринки: " + party.OwnerName + "\n";
            msg += "Название: " + party.PartyName + "\n";
            msg += "Дата и время проведения: " + party.PartyDateTimeString + "\n";
            msg += "Описание: " + party.PartyDescription + "\n";
            session.CurrentState = SessionState.Idle;
            await Bot.SendTextMessageAsync(message.Chat.Id, msg, replyMarkup: new ReplyKeyboardHide());
          }
        }
      }
    }

    private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
    {
      
      Console.WriteLine("CALLBACK "+callbackQueryEventArgs.CallbackQuery.Message.Text.ToString());
      await Bot.AnswerCallbackQueryAsync(callbackQueryEventArgs.CallbackQuery.Id,
                $"Received {callbackQueryEventArgs.CallbackQuery.Data}");
    }
    private static bool IsGreetMessage(string msg)
    {
      bool b = greetingStrings.Any(s => msg.ToLower().Contains(s));
      return b;
    }
  }
}