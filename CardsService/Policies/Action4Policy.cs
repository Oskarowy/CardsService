using CardsService.Services;

namespace CardsService.Policies
{
    public class Action4Policy : IActionPolicy
    {
        public string ActionName => "ACTION4";

        public bool IsAllowed(CardDetails cardDetails)
        {
            return true;
        }
    }
}
