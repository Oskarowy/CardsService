using CardsService.Services;

namespace CardsService.Policies
{
    public class Action5Policy : IActionPolicy
    {
        public string ActionName => "ACTION5";

        public bool IsAllowed(CardDetails cardDetails)
        {
            return cardDetails.CardType == Model.CardType.Credit;
        }
    }
}
