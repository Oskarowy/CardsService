using CardsService.Model;

namespace CardsService.Policies
{
    public class ActionAlwaysAllowedPolicy : IActionPolicy
    {
        public virtual string ActionName => "";

        public bool IsAllowed(CardDetails cardDetails)
        {
            return true;
        }
    }
}
