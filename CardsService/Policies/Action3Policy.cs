using CardsService.Services;

namespace CardsService.Policies
{
    public class Action3Policy : IActionPolicy
    {
        public string ActionName => "ACTION3";

        public bool IsAllowed(CardDetails cardDetails)
        {
            return true;
        }
    }
}
