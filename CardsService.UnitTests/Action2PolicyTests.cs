using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class Action2PolicyTests
    {
        private readonly Action2Policy _action2policy = new Action2Policy();
        private string _cardNumber = "12345";

        public Action2PolicyTests()
        {
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action2_Allow_ForAnyCardType_IfCardIsInactive_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            if (cardStatus != CardStatus.Inactive) return;

            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.True(_action2policy.IsAllowed(testedCard));
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action2_Deny_ForAnyCardType_IfCardIsOtherThanInactive_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            if (cardStatus == CardStatus.Inactive) return;

            var cardDetails = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.False(_action2policy.IsAllowed(cardDetails));
        }
    }
}
