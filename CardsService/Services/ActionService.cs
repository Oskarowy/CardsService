using CardsService.Policies;

namespace CardsService.Services;

public class ActionService
{
    private readonly IEnumerable<IActionPolicy> _actionPolicies;
    public ActionService(IEnumerable<IActionPolicy> actionPolicies)
    {
       _actionPolicies = actionPolicies;
    }

    public List<string> GetAllowedActions(CardDetails cardDetails) 
    {
        return _actionPolicies
            .Where(policy => policy.IsAllowed(cardDetails))
            .Select(policy => policy.ActionName)
            .ToList();
    }
}