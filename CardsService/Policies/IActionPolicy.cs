using CardsService.Model;

namespace CardsService.Policies
{
    public interface IActionPolicy
    {
        string ActionName { get; }

        bool IsAllowed(CardDetails cardDetails);
    }
}