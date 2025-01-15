using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;
using CardsService.Tests.Tools;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsService.Tests.ServicesIntegrationTests
{
    public class CardServiceTests
    {
        private readonly CardService _cardService;
        private readonly ActionService _actionService;
        private readonly ExternalUserCardServiceMock _externalUserCardServiceMock = new ExternalUserCardServiceMock();
        private readonly List<IActionPolicy> _policies;
        private readonly PoliciesImplementationProvider policiesImplementationProvider = new PoliciesImplementationProvider();

        public CardServiceTests()
        {
            _policies = policiesImplementationProvider.Implementations.ToList();
            _actionService = new ActionService(_policies);
            _cardService = new CardService(_externalUserCardServiceMock, _actionService);
        }

        [Theory]
        [InlineData(CardType.Credit, CardStatus.Active, false, "Card317", "User3")]
        [InlineData(CardType.Credit, CardStatus.Ordered, false, "Card315", "User3")]
        [InlineData(CardType.Credit, CardStatus.Expired, true, "Card120", "User1")]
        [InlineData(CardType.Credit, CardStatus.Blocked, false, "Card119", "User1")]
        [InlineData(CardType.Credit, CardStatus.Inactive, true, "Card116", "User1")]
        [InlineData(CardType.Credit, CardStatus.Closed, false, "Card221", "User2")]
        [InlineData(CardType.Credit, CardStatus.Restricted, true, "Card218", "User2")]
        public async Task ShouldReturnAction5_ForCreditCard_RegardlessOfCardStatus(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.Contains("ACTION5", allowedActions);
        }
    }
}
