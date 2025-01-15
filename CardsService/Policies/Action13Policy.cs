using CardsService.Model;

namespace CardsService.Policies
{
    public class Action13Policy : IActionPolicy
    {
        public string ActionName => "ACTION13";

        public bool IsAllowed(CardDetails cardDetails)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive, CardStatus.Ordered };

            return availableStatuses.Contains(cardDetails.CardStatus);
        }
    }
}
