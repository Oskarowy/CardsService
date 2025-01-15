using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;
using CardsService.Tests.Tools;

namespace CardsService.Tests.ActionPolicyUnitTests
{
    public class Action13PolicyTests
    {
        private readonly Action13Policy _action13policy = new Action13Policy();
        private string _cardNumber = "12345";

        public Action13PolicyTests()
        {
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action13_Allow_ForAnyCardType_IfCardIsActiveInActiveOrdered_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive, CardStatus.Ordered };

            if (!availableStatuses.Contains(cardStatus)) return;

            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.True(_action13policy.IsAllowed(testedCard));
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action13_Deny_ForAnyCardType_IfCardIsRestrictedExpiredClosedBlocked_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Restricted, CardStatus.Expired, CardStatus.Closed, CardStatus.Blocked };

            if (!availableStatuses.Contains(cardStatus)) return;

            var cardDetails = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.False(_action13policy.IsAllowed(cardDetails));
        }
    }
}
