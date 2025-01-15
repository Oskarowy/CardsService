using CardsService.Model;

namespace CardsService.Services;

public class CardService
{
    private readonly IExternalUserCardService _externalUserCardService;
    private readonly ActionService _actionService;
    public CardService(IExternalUserCardService externalUserCardService, ActionService actionService)
    {
        _externalUserCardService = externalUserCardService;
        _actionService = actionService;
    }
    public async Task<CardDetails?> GetCardDetails(string userId, string cardNumber)
    {
        // At this point, we would typically make an HTTP call to an external service
        // to fetch the data. For this example we use generated sample data.
        var userCards = await _externalUserCardService.GetUserCards();

        if (!userCards.TryGetValue(userId, out var cards))
            throw new KeyNotFoundException("Inorrect UserId : " + userId);

        if (!cards.TryGetValue(cardNumber, out var cardDetails))
            throw new ArgumentException("Card with number " + cardNumber + " not found for user " + userId);
        return cardDetails;
    }

    public async Task<List<string>> GetCardAllowedActions(string userId, string cardNumber)
    {
        try
        {
            var cardDetails = await GetCardDetails(userId, cardNumber);
            return _actionService.GetAllowedActions(cardDetails);
        }
        catch (Exception)
        {
            throw;
        }
    }
   
}