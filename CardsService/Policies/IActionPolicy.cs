using CardsService.Services;

namespace CardsService.Policies
{
    public interface IActionPolicy
    {
        string ActionName { get; }

        bool IsAllowed(CardDetails cardDetails);
    }
}