using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTelegramBot
{
  class PartyManager
  {
    public static List<Party> Parties;

    public static Party GetPartyById(int partyId)
    {
      if (Parties == null)
        Parties = new List<Party>();
      Party party = Parties.FirstOrDefault(m => m.OwnerId == partyId);
      if (party != null)
      {
        Console.WriteLine("Found active party for user " + partyId);
        SaveParties();
        return party;
      }
      else
      {
        Console.WriteLine("Couldn't find party for user " + partyId + ". Creating new one.");
        party = new Party(partyId);
        Parties.Add(party);
        SaveParties();
        return party;
      }
    }

    public static void EditOrCreateParty(Party party)
    {
      Party current = GetPartyById(party.OwnerId);

      current.OwnerName = party.OwnerName;
      current.PartyName = party.PartyName;
      current.PartyDescription = party.PartyDescription;
      current.PartyDateTimeString = party.PartyDateTimeString;
      SaveParties();
    }

    public static void SaveParties()
    {

    }
  }
}
