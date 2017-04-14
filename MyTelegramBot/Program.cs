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
    private static List<string> greetingStrings;
    static void Main(string[] args)
    {
      wb = new WordBase();
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

      if ((message.Chat.Type == ChatType.Supergroup || message.Chat.Type == ChatType.Supergroup) && IsGreetMessage(message.Text))
      {
        string greetingMessage = wb.GenerateGreetingMessage(true);

        await Bot.SendTextMessageAsync(message.Chat.Id, greetingMessage,
            replyMarkup: new ReplyKeyboardHide());
      }
      if ((message.Chat.Type == ChatType.Supergroup || message.Chat.Type == ChatType.Supergroup) 
        && (message.Text.ToLower().Contains("федот") || message.Text.ToLower().Contains("манул")))
      {
        string msg = "Манул, кстати, еще тот говноед!";

        await Bot.SendTextMessageAsync(message.Chat.Id, msg,
            replyMarkup: new ReplyKeyboardHide());
      }

      if ((message.Chat.Type == ChatType.Supergroup || message.Chat.Type == ChatType.Supergroup)
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
      else if(message.Text.StartsWith("/help"))
        {
            var usage = @"Usage:
/hello   - bot will say hello to you creatively =)
";

            await Bot.SendTextMessageAsync(message.Chat.Id, usage,
                replyMarkup: new ReplyKeyboardHide());
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