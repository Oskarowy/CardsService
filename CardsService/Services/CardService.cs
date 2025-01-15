using CardsService.Model;

namespace CardsService.Services;

public class CardService
{
    private readonly IExternalUserCardService _externalUserCardService;
    public CardService(IExternalUserCardService externalUserCardService)
    {
        _externalUserCardService = externalUserCardService;
    }
    public async Task<CardDetails?> GetCardDetails(string userId, string cardNumber)
    {
        // At this point, we would typically make an HTTP call to an external service
        // to fetch the data. For this example we use generated sample data.
        var userCards = await _externalUserCardService.GetUserCards();

        if (!userCards.TryGetValue(userId, out var cards)
        || !cards.TryGetValue(cardNumber, out var cardDetails))
        {
            return null;
        }
        return cardDetails;
    }
   
}