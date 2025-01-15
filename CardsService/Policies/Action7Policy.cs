using CardsService.Model;
using CardsService.Services;

namespace CardsService.Policies
{
    public class Action7Policy : IActionPolicy
    {
        public string ActionName => "ACTION7";

        public bool IsAllowed(CardDetails cardDetails)
        {
            var alwaysBlockedStatuses = new List<CardStatus> { CardStatus.Restricted, CardStatus.Expired, CardStatus.Closed };
            var pinDependingStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive, CardStatus.Ordered };

            if (cardDetails.CardStatus == CardStatus.Blocked)
                return cardDetails.IsPinSet;

            if (alwaysBlockedStatuses.Contains(cardDetails.CardStatus)) return false;

            if (pinDependingStatuses.Contains(cardDetails.CardStatus))
                return !cardDetails.IsPinSet;

            throw new NotImplementedException();
        }
    }
}
