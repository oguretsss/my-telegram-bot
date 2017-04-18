namespace MyTelegramBot
{
  public class UserSession
  {
    private int sessionId;

    public SessionState CurrentState { get; set; }
    public string LastMessage { get; set; }
    public int SessionId { get { return sessionId; } }

    public UserSession(int userId)
    {
      sessionId = userId;
      CurrentState = SessionState.Idle;
    }
  }

  public enum SessionState
  {
    Idle,
    CreateParty,
    EditParty,
    JoinParty
  }
}
