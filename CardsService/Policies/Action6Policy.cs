using CardsService.Model;
using CardsService.Services;

namespace CardsService.Policies
{
    public class Action6Policy : IActionPolicy
    {
        public string ActionName => "ACTION6";

        public bool IsAllowed(CardDetails cardDetails)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive, CardStatus.Ordered };

            if (availableStatuses.Contains(cardDetails.CardStatus))
                return cardDetails.IsPinSet;

            return true;
        }
    }
}
