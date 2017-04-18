using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTelegramBot
{
  public class SessionManager
  {
    public static List<UserSession> Sessions;

    public static UserSession GetSessionByUserId(int userId)
    {
      if (Sessions == null)
        Sessions = new List<UserSession>();
      UserSession sess = Sessions.FirstOrDefault(m => m.SessionId == userId);
      if (sess != null)
      {
        Console.WriteLine("Found active session for user "+userId);
        SaveSessions();
        return sess;
      }
      else
      {
        Console.WriteLine("Couldn't find session for user " + userId + ". Creating new one.");
        sess = new UserSession(userId);
        Sessions.Add(sess);
        SaveSessions();
        return sess;
      }
    }

    public static void DeleteSession(int userId)
    {
      UserSession sess = Sessions.FirstOrDefault(m => m.SessionId == userId);
      Sessions.RemoveAll(m => m.SessionId == userId);
      Console.WriteLine("Successfully deleted session for user " + userId);
      SaveSessions();
    }

    public static void SaveSessions()
    {

    }
  }
}
