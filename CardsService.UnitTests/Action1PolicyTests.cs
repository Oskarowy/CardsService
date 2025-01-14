using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class Action1PolicyTests
    {
        private readonly Action1Policy _action1policy = new Action1Policy();
        private string _cardNumber = "12345";

        public Action1PolicyTests()
        {
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action1_Allow_ForAnyCardType_IfCardIsActive_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            if (cardStatus != CardStatus.Active) return;

            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.True(_action1policy.IsAllowed(testedCard));
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action1_Deny_ForAnyCardType_IfCardIsNotActive_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            if (cardStatus == CardStatus.Active) return;

            var cardDetails = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.False(_action1policy.IsAllowed(cardDetails));
        }
    }
}
