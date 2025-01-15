using CardsService.Model;

namespace CardsService.Services
{
    public interface IExternalUserCardService
    {
        public Task<Dictionary<string, Dictionary<string, CardDetails>>> GetUserCards();
    }
}
