using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;
using CardsService.Tests.Tools;

namespace CardsService.Tests.ActionPolicyUnitTests
{
    public class Action11PolicyTests
    {
        private readonly Action11Policy _action11policy = new Action11Policy();
        private string _cardNumber = "12345";

        public Action11PolicyTests()
        {
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action11_Allow_ForAnyCardType_IfCardIsActiveInActive_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive };

            if (!availableStatuses.Contains(cardStatus)) return;

            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.True(_action11policy.IsAllowed(testedCard));
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action11_Deny_ForAnyCardType_IfCardIsRestrictedExpiredClosedBlockedOrdered_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Restricted, CardStatus.Expired, CardStatus.Closed, CardStatus.Blocked, CardStatus.Ordered };

            if (!availableStatuses.Contains(cardStatus)) return;

            var cardDetails = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.False(_action11policy.IsAllowed(cardDetails));
        }
    }
}
