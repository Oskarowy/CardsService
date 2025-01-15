using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;
using CardsService.Tests.Tools;

namespace CardsService.Tests.ActionPolicyUnitTests
{
    public class Action7PolicyTests
    {
        private readonly Action7Policy _action7policy = new Action7Policy();
        private string _cardNumber = "12345";

        public Action7PolicyTests()
        {
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action7_Allow_ForEveryCardType_IfCardIsActiveInActiveOrdered_AndPinNotSet(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive, CardStatus.Ordered };

            if (!availableStatuses.Contains(cardStatus) || isPinSet) return;

            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);
            var policyResult = _action7policy.IsAllowed(testedCard);
            Assert.Equal(!testedCard.IsPinSet, policyResult);
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action7_Deny_ForAnyCardType_IfCardIsActiveInActiveOrdered_AndPinIsSet(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive, CardStatus.Ordered };

            if (!availableStatuses.Contains(cardStatus) || !isPinSet) return;

            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);
            var policyResult = _action7policy.IsAllowed(testedCard);
            Assert.Equal(!testedCard.IsPinSet, policyResult);
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action7_Deny_ForEveryCardType_IfCardIsRestrictedExpiredClosed_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Restricted, CardStatus.Expired, CardStatus.Closed };

            if (!availableStatuses.Contains(cardStatus)) return;

            var cardDetails = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.False(_action7policy.IsAllowed(cardDetails));
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action7_Deny_ForEveryCardType_IfCardIsBlocked_AndPinNotSet(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            if (cardStatus != CardStatus.Blocked || isPinSet) return;

            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);
            var policyResult = _action7policy.IsAllowed(testedCard);
            Assert.Equal(testedCard.IsPinSet, policyResult);
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action7_Allow_ForEveryCardType_IfCardIsBlocked_AndPinIsSet(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            if (cardStatus != CardStatus.Blocked || !isPinSet) return;

            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);
            var policyResult = _action7policy.IsAllowed(testedCard);
            Assert.Equal(testedCard.IsPinSet, policyResult);
        }
    }
}
