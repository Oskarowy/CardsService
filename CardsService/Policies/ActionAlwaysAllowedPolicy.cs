using CardsService.Services;

namespace CardsService.Policies
{
    public class ActionAlwaysAllowedPolicy : IActionPolicy
    {
        public virtual string ActionName => throw new NotImplementedException();

        public bool IsAllowed(CardDetails cardDetails)
        {
            return true;
        }
    }
}
