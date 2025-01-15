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

        #region Action1 - only for Active cards
        [Theory]
        [InlineData(CardType.Credit, CardStatus.Active, false, "Card317", "User3")]
        [InlineData(CardType.Debit, CardStatus.Active, true, "Card110", "User1")]
        [InlineData(CardType.Prepaid, CardStatus.Active, false, "Card13", "User1")]
        public async Task ShouldReturnAction1_ForEveryCardType_OnlyForActiveCards(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.Contains("ACTION1", allowedActions);
        }

        [Theory]
        [InlineData(CardType.Prepaid, CardStatus.Ordered, false, "Card11", "User1")]
        [InlineData(CardType.Debit, CardStatus.Closed, true, "Card314", "User3")]
        [InlineData(CardType.Debit, CardStatus.Restricted, false, "Card211", "User2")]
        [InlineData(CardType.Credit, CardStatus.Closed, false, "Card221", "User2")]
        [InlineData(CardType.Credit, CardStatus.Restricted, true, "Card218", "User2")]
        public async Task ShouldNotReturnAction1_ForAnyCardType_IfCardIsNotActive(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.DoesNotContain("ACTION1", allowedActions);
        }
        #endregion
        #region Action2 - only for Inactive cards
        [Theory]
        [InlineData(CardType.Prepaid, CardStatus.Inactive, true, "Card32", "User3")]
        [InlineData(CardType.Debit, CardStatus.Inactive, false, "Card29", "User2")]
        [InlineData(CardType.Credit, CardStatus.Inactive, true, "Card116", "User1")]
        public async Task ShouldReturnAction2_ForEveryCardType_OnlyForInctiveCards(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.Contains("ACTION2", allowedActions);
        }

        [Theory]
        [InlineData(CardType.Prepaid, CardStatus.Active, false, "Card13", "User1")]
        [InlineData(CardType.Debit, CardStatus.Blocked, true, "Card312", "User3")]
        [InlineData(CardType.Debit, CardStatus.Restricted, false, "Card211", "User2")]
        [InlineData(CardType.Credit, CardStatus.Active, false, "Card317", "User3")]
        [InlineData(CardType.Credit, CardStatus.Restricted, true, "Card218", "User2")]
        public async Task ShouldNotReturnAction2_ForAnyCardType_IfCardIsOtherThanInactive(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.DoesNotContain("ACTION2", allowedActions);
        }
        #endregion
        #region Action3, Action4, Action9 - no matter what
        [Theory]
        [InlineData(CardType.Credit, CardStatus.Blocked, false, "Card119", "User1")]
        [InlineData(CardType.Credit, CardStatus.Inactive, true, "Card116", "User1")]
        [InlineData(CardType.Prepaid, CardStatus.Expired, true, "Card26", "User2")]
        [InlineData(CardType.Prepaid, CardStatus.Blocked, false, "Card25", "User2")]
        [InlineData(CardType.Debit, CardStatus.Blocked, true, "Card312", "User3")]
        [InlineData(CardType.Debit, CardStatus.Inactive, false, "Card29", "User2")]
        public async Task ShouldReturnAction3and4and9_ForEveryCardType_RegardlessOfCardStatus(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.Contains("ACTION3", allowedActions);
            Assert.Contains("ACTION4", allowedActions);
            Assert.Contains("ACTION9", allowedActions);
        }
        #endregion
        #region Action5 - only for CreditCards
        [Theory]
        [InlineData(CardType.Credit, CardStatus.Active, false, "Card317", "User3")]   
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

        [Theory]
        [InlineData(CardType.Prepaid, CardStatus.Active, false, "Card13", "User1")]
        [InlineData(CardType.Prepaid, CardStatus.Ordered, false, "Card11", "User1")]      
        [InlineData(CardType.Debit, CardStatus.Closed, true, "Card314", "User3")]
        [InlineData(CardType.Debit, CardStatus.Restricted, false, "Card211", "User2")]
        public async Task ShouldNotReturnAction5_ForDebitAndPrepaidCard_RegardlessOfCardStatus(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.DoesNotContain("ACTION5", allowedActions);
        }
        #endregion
        #region Action8 - for Ordered, Active, Blocked and Inactive cards
        [Theory]
        [InlineData(CardType.Debit, CardStatus.Ordered, true, "Card18", "User1")]
        [InlineData(CardType.Prepaid, CardStatus.Ordered, false, "Card11", "User1")]
        [InlineData(CardType.Credit, CardStatus.Ordered, false, "Card315", "User3")]
        public async Task ShouldReturnAction8_ForEveryCardType_IfCardIsOrdered(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.Contains("ACTION8", allowedActions);
        }

        [Theory]
        [InlineData(CardType.Credit, CardStatus.Active, false, "Card317", "User3")]
        [InlineData(CardType.Debit, CardStatus.Active, true, "Card110", "User1")]
        [InlineData(CardType.Prepaid, CardStatus.Active, false, "Card13", "User1")]
        public async Task ShouldReturnAction8_ForEveryCardType_IfCardIsActive(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.Contains("ACTION8", allowedActions);
        }

        [Theory]
        [InlineData(CardType.Prepaid, CardStatus.Inactive, true, "Card32", "User3")]
        [InlineData(CardType.Debit, CardStatus.Inactive, false, "Card29", "User2")]
        [InlineData(CardType.Credit, CardStatus.Inactive, true, "Card116", "User1")]
        public async Task ShouldReturnAction8_ForEveryCardType_IfCardIsInactive(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.Contains("ACTION8", allowedActions);
        }
        [Theory]
        [InlineData(CardType.Prepaid, CardStatus.Blocked, false, "Card25", "User2")]       
        [InlineData(CardType.Debit, CardStatus.Blocked, true, "Card312", "User3")]
        [InlineData(CardType.Credit, CardStatus.Blocked, false, "Card119", "User1")] 
        public async Task ShouldReturnAction8_ForEveryCardType_IfCardIsBlocked(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.Contains("ACTION8", allowedActions);
        }

        [Theory]
        [InlineData(CardType.Debit, CardStatus.Restricted, false, "Card211", "User2")]
        [InlineData(CardType.Credit, CardStatus.Restricted, true, "Card218", "User2")]
        public async Task ShouldNotReturnAction8_ForAnyCardType_IfCardIsRestricted(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.DoesNotContain("ACTION8", allowedActions);
        }
        
        [Theory]
        [InlineData(CardType.Prepaid, CardStatus.Expired, true, "Card26", "User2")]
        [InlineData(CardType.Debit, CardStatus.Expired, false, "Card313", "User3")]
        public async Task ShouldNotReturnAction8_ForAnyCardType_IfCardIsExpired(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.DoesNotContain("ACTION8", allowedActions);
        }
        [Theory]
        [InlineData(CardType.Prepaid, CardStatus.Closed, false, "Card37", "User3")]
        [InlineData(CardType.Debit, CardStatus.Closed, true, "Card314", "User3")]
        public async Task ShouldNotReturnAction8_ForAnyCardType_IfCardIsClosed(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.DoesNotContain("ACTION8", allowedActions);
        }
        #endregion
        #region Action11 - for Active and Inactive cards       
        [Theory]
        [InlineData(CardType.Credit, CardStatus.Active, false, "Card317", "User3")]
        [InlineData(CardType.Debit, CardStatus.Active, true, "Card110", "User1")]
        [InlineData(CardType.Prepaid, CardStatus.Active, false, "Card13", "User1")]
        public async Task ShouldReturnAction11_ForEveryCardType_IfCardIsActive(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.Contains("ACTION11", allowedActions);
        }

        [Theory]
        [InlineData(CardType.Prepaid, CardStatus.Inactive, true, "Card32", "User3")]
        [InlineData(CardType.Debit, CardStatus.Inactive, false, "Card29", "User2")]
        [InlineData(CardType.Credit, CardStatus.Inactive, true, "Card116", "User1")]
        public async Task ShouldReturnAction11_ForEveryCardType_IfCardIsInactive(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.Contains("ACTION11", allowedActions);
        }
        [Theory]
        [InlineData(CardType.Prepaid, CardStatus.Blocked, false, "Card25", "User2")]
        [InlineData(CardType.Debit, CardStatus.Blocked, true, "Card312", "User3")]
        [InlineData(CardType.Debit, CardStatus.Restricted, false, "Card211", "User2")]
        [InlineData(CardType.Credit, CardStatus.Restricted, true, "Card218", "User2")]
        [InlineData(CardType.Prepaid, CardStatus.Expired, true, "Card26", "User2")]
        [InlineData(CardType.Debit, CardStatus.Closed, true, "Card314", "User3")]

        public async Task ShouldNotReturnAction11_ForEveryCardType_IfCardIsNotActiveOrInactive(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.DoesNotContain("ACTION11", allowedActions);
        }   
        #endregion
        #region Action10, Action12, Action13 - for Ordered, Active and Inactive cards
        [Theory]
        [InlineData(CardType.Debit, CardStatus.Ordered, true, "Card18", "User1")]
        [InlineData(CardType.Prepaid, CardStatus.Ordered, false, "Card11", "User1")]
        [InlineData(CardType.Credit, CardStatus.Ordered, false, "Card315", "User3")]
        public async Task ShouldReturnAction10and12and13_ForEveryCardType_IfCardIsOrdered(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.Contains("ACTION10", allowedActions);
            Assert.Contains("ACTION12", allowedActions);
            Assert.Contains("ACTION13", allowedActions);
        }

        [Theory]
        [InlineData(CardType.Credit, CardStatus.Active, false, "Card317", "User3")]
        [InlineData(CardType.Debit, CardStatus.Active, true, "Card110", "User1")]
        [InlineData(CardType.Prepaid, CardStatus.Active, false, "Card13", "User1")]
        public async Task ShouldReturnAction10and12and13_ForEveryCardType_IfCardIsActive(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.Contains("ACTION10", allowedActions);
            Assert.Contains("ACTION12", allowedActions);
            Assert.Contains("ACTION13", allowedActions);
        }

        [Theory]
        [InlineData(CardType.Prepaid, CardStatus.Inactive, true, "Card32", "User3")]
        [InlineData(CardType.Debit, CardStatus.Inactive, false, "Card29", "User2")]
        [InlineData(CardType.Credit, CardStatus.Inactive, true, "Card116", "User1")]
        public async Task ShouldReturnAction10and12and13_ForEveryCardType_IfCardIsInactive(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.Contains("ACTION10", allowedActions);
            Assert.Contains("ACTION12", allowedActions);
            Assert.Contains("ACTION13", allowedActions);
        }

        [Theory]
        [InlineData(CardType.Debit, CardStatus.Restricted, false, "Card211", "User2")]
        [InlineData(CardType.Credit, CardStatus.Restricted, true, "Card218", "User2")]
        public async Task ShouldNotReturnAction10or12or13_ForAnyCardType_IfCardIsRestricted(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.DoesNotContain("ACTION10", allowedActions);
            Assert.DoesNotContain("ACTION12", allowedActions);
            Assert.DoesNotContain("ACTION13", allowedActions);
        }
        [Theory]
        [InlineData(CardType.Prepaid, CardStatus.Blocked, false, "Card25", "User2")]  
        [InlineData(CardType.Debit, CardStatus.Blocked, true, "Card312", "User3")]
        public async Task ShouldNotReturnAction10or12or13_ForAnyCardType_IfCardIsBlocked(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.DoesNotContain("ACTION10", allowedActions);
            Assert.DoesNotContain("ACTION12", allowedActions);
            Assert.DoesNotContain("ACTION13", allowedActions);
        }
        [Theory]
        [InlineData(CardType.Prepaid, CardStatus.Expired, true, "Card26", "User2")]
        [InlineData(CardType.Debit, CardStatus.Expired, false, "Card313", "User3")]
        public async Task ShouldNotReturnAction10or12or13_ForAnyCardType_IfCardIsExpired(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.DoesNotContain("ACTION10", allowedActions);
            Assert.DoesNotContain("ACTION12", allowedActions);
            Assert.DoesNotContain("ACTION13", allowedActions);
        }
        [Theory]
        [InlineData(CardType.Prepaid, CardStatus.Closed, false, "Card37", "User3")]
        [InlineData(CardType.Debit, CardStatus.Closed, true, "Card314", "User3")]
        public async Task ShouldNotReturnAction10or12or13_ForAnyCardType_IfCardIsClosed(CardType cardType, CardStatus cardStatus, bool isPinSet, string cardNumber, string userId)
        {
            var cardDetails = await _cardService.GetCardDetails(userId, cardNumber);
            var expectedCardDetails = new CardDetails(cardNumber, cardType, cardStatus, isPinSet);
            var allowedActions = await _cardService.GetCardAllowedActions(userId, cardNumber);

            Assert.NotNull(allowedActions);
            Assert.NotNull(cardDetails);

            Assert.Equal(expectedCardDetails, cardDetails);
            Assert.DoesNotContain("ACTION10", allowedActions);
            Assert.DoesNotContain("ACTION12", allowedActions);
            Assert.DoesNotContain("ACTION13", allowedActions);
        }
        #endregion
    }
}
