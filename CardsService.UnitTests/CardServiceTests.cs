using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;
using Moq;

namespace CardsService.UnitTests
{
    public class CardServiceTests
    {
        private readonly CardService _cardService;
        private readonly Mock<IExternalUserCardService> _externalUserCardService = new Mock<IExternalUserCardService>();

        public CardServiceTests()
        {        
            _cardService = new CardService(_externalUserCardService.Object);
        }

        [Theory]
        [InlineData(CardType.Prepaid, CardStatus.Active, false, "Card13", "User1")]
        [InlineData(CardType.Prepaid, CardStatus.Ordered, false, "Card11", "User1")]
        [InlineData(CardType.Prepaid, CardStatus.Expired, true, "Card26", "User2")]
        [InlineData(CardType.Prepaid, CardStatus.Blocked, false, "Card25", "User2")]
        [InlineData(CardType.Prepaid, CardStatus.Inactive, true, "Card32", "User3")]
        [InlineData(CardType.Prepaid, CardStatus.Closed, false, "Card37", "User3")]
        [InlineData(CardType.Prepaid, CardStatus.Restricted, true, "Card34", "User3")]
        [InlineData(CardType.Debit, CardStatus.Active, true, "Card110", "User1")]
        [InlineData(CardType.Debit, CardStatus.Ordered, true, "Card18", "User1")]
        [InlineData(CardType.Debit, CardStatus.Expired, false, "Card313", "User3")]
        [InlineData(CardType.Debit, CardStatus.Blocked, true, "Card312", "User3")]
        [InlineData(CardType.Debit, CardStatus.Inactive, false, "Card29", "User2")]
        [InlineData(CardType.Debit, CardStatus.Closed, true, "Card314", "User3")]
        [InlineData(CardType.Debit, CardStatus.Restricted, false, "Card211", "User2")]
        [InlineData(CardType.Credit, CardStatus.Active, false, "Card317", "User3")]
        [InlineData(CardType.Credit, CardStatus.Ordered, false, "Card315", "User3")]
        [InlineData(CardType.Credit, CardStatus.Expired, true, "Card120", "User1")]
        [InlineData(CardType.Credit, CardStatus.Blocked, false, "Card119", "User1")]
        [InlineData(CardType.Credit, CardStatus.Inactive, true, "Card116", "User1")]
        [InlineData(CardType.Credit, CardStatus.Closed, false, "Card221", "User2")]
        [InlineData(CardType.Credit, CardStatus.Restricted, true, "Card218", "User2")]
        public async Task ShouldReturnCardDetails_WhenCardExists(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var userCards = new Dictionary<string, Dictionary<string, CardDetails>>
            {
                { userId, new Dictionary<string, CardDetails> { { cardNumber, expectedCardDetails } } }
            };

            _externalUserCardService.Setup(m => m.GetUserCards()).ReturnsAsync(userCards);

            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            Assert.NotNull(cardDetails);
            Assert.Equal(expectedCardDetails, cardDetails);
            _externalUserCardService.Verify(x => x.GetUserCards(), Times.Once());
        }

        [Fact]
        public async Task ShouldThrow_ArgumentException_WhenCardDoesNotExists()
        {
            var userCards = new Dictionary<string, Dictionary<string, CardDetails>>();

            _externalUserCardService.Setup(m => m.GetUserCards()).ReturnsAsync(userCards);

            await Assert.ThrowsAsync<ArgumentException>(() => _cardService.GetCardDetails("User12345", "NonExistentCardNumber"));
        }
    }
}
