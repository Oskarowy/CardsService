using CardsService.Model;

namespace CardsService.Policies
{
    public class Action10Policy : IActionPolicy
    {
        public string ActionName => "ACTION10";

        public bool IsAllowed(CardDetails cardDetails)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive, CardStatus.Ordered };

            return availableStatuses.Contains(cardDetails.CardStatus);
        }
    }
}
