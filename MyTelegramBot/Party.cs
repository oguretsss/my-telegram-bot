using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTelegramBot
{
  class Party
  {
    private int ownerId;
    public string OwnerName { get; set; }
    public string PartyName { get; set; }
    public string PartyDateTimeString { get; set; }
    public string PartyDescription { get; set; }
    public int OwnerId { get { return ownerId; } }

    public Party(int owner)
    {
      ownerId = owner;
    }
    public Party(int owner, string ownerName, string partyName, string dateTime, string description)
    {
      ownerId = owner;
      OwnerName = ownerName;
      PartyName = partyName;
      PartyDateTimeString = dateTime;
      PartyDescription = description;
    }
  }
}
