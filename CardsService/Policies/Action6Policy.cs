using CardsService.Model;

namespace CardsService.Policies
{
    public class Action6Policy : IActionPolicy
    {
        public string ActionName => "ACTION6";

        public bool IsAllowed(CardDetails cardDetails)
        {
            var alwaysBlockedStatuses = new List<CardStatus> { CardStatus.Restricted, CardStatus.Expired, CardStatus.Closed };
            var pinDependingStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive, CardStatus.Ordered, CardStatus.Blocked };
            
            if (alwaysBlockedStatuses.Contains(cardDetails.CardStatus)) return false;

            if (pinDependingStatuses.Contains(cardDetails.CardStatus))
                return cardDetails.IsPinSet;

            throw new NotImplementedException();
        }
    }
}
