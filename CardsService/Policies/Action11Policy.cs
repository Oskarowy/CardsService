using CardsService.Model;

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
