using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;
using CardsService.Tests.Tools;

namespace CardsService.Tests.ActionPolicyUnitTests
{
    public class Action5PolicyTests
    {
        private readonly Action5Policy _action5policy = new Action5Policy();
        private string _cardNumber = "12345";

        public Action5PolicyTests()
        {
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action5_Allow_ForCreditCard_ForAnyCardStatus_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            if (cardType != CardType.Credit) return;

            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.True(_action5policy.IsAllowed(testedCard));
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action5_Deny_ForDebitAndPrepaidCard_ForEveryCardStatus_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            if (cardType == CardType.Credit) return;

            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.False(_action5policy.IsAllowed(testedCard));
        }
    }
}
