using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class Action6PolicyTests
    {
        private readonly Action6Policy _action6policy = new Action6Policy();
        private string _cardNumber = "12345";

        public Action6PolicyTests()
        {
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action6_Deny_ForEveryCardType_IfCardIsActiveInActiveOrdered_AndPinNotSet(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive, CardStatus.Ordered };

            if (!availableStatuses.Contains(cardStatus) || isPinSet) return;

            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);
            var policyResult = _action6policy.IsAllowed(testedCard);
            Assert.Equal(testedCard.IsPinSet, policyResult);
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action6_Allow_ForAnyCardType_IfCardIsActiveInActiveOrdered_AndPinIsSet(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive, CardStatus.Ordered };

            if (!availableStatuses.Contains(cardStatus) || !isPinSet) return;

            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);
            var policyResult = _action6policy.IsAllowed(testedCard);
            Assert.Equal(testedCard.IsPinSet, policyResult);
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action6_Deny_ForEveryCardType_IfCardIsRestrictedExpiredClosed_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Restricted, CardStatus.Expired, CardStatus.Closed };

            if (!availableStatuses.Contains(cardStatus)) return;

            var cardDetails = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.False(_action6policy.IsAllowed(cardDetails));
        }
    }
}
