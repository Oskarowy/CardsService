using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;
using CardsService.Tests.Tools;

namespace CardsService.Tests.ActionPolicyUnitTests
{
    public class Action10PolicyTests
    {
        private readonly Action10Policy _action10policy = new Action10Policy();
        private string _cardNumber = "12345";

        public Action10PolicyTests()
        {
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action10_Allow_ForAnyCardType_IfCardIsActiveInActiveOrdered_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive, CardStatus.Ordered };

            if (!availableStatuses.Contains(cardStatus)) return;

            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.True(_action10policy.IsAllowed(testedCard));
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action10_Deny_ForAnyCardType_IfCardIsRestrictedExpiredClosedBlocked_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Restricted, CardStatus.Expired, CardStatus.Closed, CardStatus.Blocked };

            if (!availableStatuses.Contains(cardStatus)) return;

            var cardDetails = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.False(_action10policy.IsAllowed(cardDetails));
        }
    }
}
