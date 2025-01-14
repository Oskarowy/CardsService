using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class Action12PolicyTests
    {
        private readonly Action12Policy _action12policy = new Action12Policy();
        private string _cardNumber = "12345";

        public Action12PolicyTests()
        {
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action12_Allow_ForAnyCardType_IfCardIsActiveInActiveOrdered_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive, CardStatus.Ordered };

            if (!availableStatuses.Contains(cardStatus)) return;

            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.True(_action12policy.IsAllowed(testedCard));
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action12_Deny_ForAnyCardType_IfCardIsRestrictedExpiredClosedBlocked_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Restricted, CardStatus.Expired, CardStatus.Closed, CardStatus.Blocked };

            if (!availableStatuses.Contains(cardStatus)) return;

            var cardDetails = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.False(_action12policy.IsAllowed(cardDetails));
        }
    }
}
