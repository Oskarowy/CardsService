using CardsService.Model;
using CardsService.Services;
using System.Reflection.Metadata.Ecma335;

namespace CardsService.Policies
{
    public class Action11Policy : IActionPolicy
    {
        public string ActionName => "ACTION11";

        public bool IsAllowed(CardDetails cardDetails)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive };

            return availableStatuses.Contains(cardDetails.CardStatus);
        }
    }
}
