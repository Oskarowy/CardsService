using CardsService.Services;

namespace CardsService.Policies
{
    public class Action1Policy : IActionPolicy
    {
        public string ActionName => "ACTION1";

        public bool IsAllowed(CardDetails cardDetails)
        {
            return cardDetails.CardStatus == Model.CardStatus.Active;
        }
    }
}
