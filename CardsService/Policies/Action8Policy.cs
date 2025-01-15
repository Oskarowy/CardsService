using CardsService.Model;

namespace CardsService.Policies
{
    public class Action8Policy : IActionPolicy
    {
        public string ActionName => "ACTION8";

        public bool IsAllowed(CardDetails cardDetails)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive, CardStatus.Ordered, CardStatus.Blocked };

            return availableStatuses.Contains(cardDetails.CardStatus);
        }
    }
}
