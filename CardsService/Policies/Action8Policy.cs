using CardsService.Services;

namespace CardsService.Policies
{
    public class Action8Policy : IActionPolicy
    {
        public string ActionName => "ACTION8";

        public bool IsAllowed(CardDetails cardDetails)
        {
            return cardDetails.CardStatus == Model.CardStatus.Active;
        }
    }
}
