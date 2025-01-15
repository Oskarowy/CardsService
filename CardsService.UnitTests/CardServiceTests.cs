using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class CardServiceTests
    {
        private readonly CardService _cardService;

        public CardServiceTests()
        {
            _cardService = new CardService();
        }

        [Fact]
        public async Task ShouldReturnCardDetails_WhenCardExists()
        {
            CardType cardType = CardType.Prepaid;
            CardStatus cardStatus = CardStatus.Active;
            string cardNumber = "Card13";
            string userId = "User1";
            bool isPinSet = false;
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);

            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            Assert.NotNull(cardDetails);
            Assert.Equal(expectedCardDetails, cardDetails);
        }
    }
}
