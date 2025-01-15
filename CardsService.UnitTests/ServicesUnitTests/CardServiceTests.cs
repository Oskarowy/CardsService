using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;
using CardsService.Tests.Tools;
using Microsoft.VisualBasic;
using Moq;
using System.Runtime.CompilerServices;

namespace CardsService.Tests.ServicesUnitTests
{
    public class CardServiceTests
    {
        private readonly CardService _cardService;
        private readonly Mock<IExternalUserCardService> _externalUserCardService = new Mock<IExternalUserCardService>();
        private readonly Mock<ActionService> _actionService;

        public CardServiceTests()
        {
            _actionService = new Mock<ActionService>(new List<IActionPolicy> { new FakePolicy1(), new FakePolicy2(), new FakePolicy3() });
            _cardService = new CardService(_externalUserCardService.Object, _actionService.Object);
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
            string userId = "User1";
            var userCards = new Dictionary<string, Dictionary<string, CardDetails>>
            {
                { userId, new Dictionary<string, CardDetails>() }
            };
            _externalUserCardService.Setup(m => m.GetUserCards()).ReturnsAsync(userCards);

            await Assert.ThrowsAsync<ArgumentException>(() => _cardService.GetCardDetails("User1", "NonExistentCardNumber"));
        }

        [Fact]
        public async Task ShouldThrow_KeyNotFoundException_WhenUserDoesNotExists()
        {
            string existingUser = "User1";
            string notExistingUser = "User12345";
            string cardNumber = "ExistingCardNumber";
            var expectedCardDetails = new CardDetails(cardNumber, CardType.Credit, CardStatus.Active, true);

            var userCards = new Dictionary<string, Dictionary<string, CardDetails>>
            {
                { existingUser, new Dictionary<string, CardDetails> { { cardNumber, expectedCardDetails } } }
            };
            _externalUserCardService.Setup(m => m.GetUserCards()).ReturnsAsync(userCards);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _cardService.GetCardDetails(notExistingUser, cardNumber));
        }

        [Fact]
        public async Task ShouldReturnAllowedActions_WhenCardExists()
        {
            string userId = "User1";
            string cardNumber = "ExistingCardNumber";
            var expectedCardDetails = new CardDetails(cardNumber, CardType.Credit, CardStatus.Active, true);
            var userCards = new Dictionary<string, Dictionary<string, CardDetails>>
            {
                { userId, new Dictionary<string, CardDetails> { { cardNumber, expectedCardDetails } } }
            };
            var expectedActions = new List<string> { "FAKE_ACTION1", "FAKE_ACTION2", "FAKE_ACTION3" };

            _externalUserCardService.Setup(m => m.GetUserCards()).ReturnsAsync(userCards);

            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);
            Assert.NotNull(allowedActions);
            Assert.Equal(expectedActions, allowedActions);
            _externalUserCardService.Verify(x => x.GetUserCards(), Times.Once());
        }
    }
}
