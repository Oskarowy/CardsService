using CardsService.Model;

namespace CardsService.Policies
{
    public class Action2Policy : IActionPolicy
    {
        public string ActionName => "ACTION2";

        public bool IsAllowed(CardDetails cardDetails)
        {
            return cardDetails.CardStatus == Model.CardStatus.Inactive;
        }
    }
}
