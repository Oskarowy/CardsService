using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class Action9PolicyTests
    {
        private readonly Action9Policy _action9policy = new Action9Policy();
        private string _cardNumber = "12345";

        public Action9PolicyTests()
        {
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action9_Allow_ForAnyCardType_ForAnyCardStatus_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.True(_action9policy.IsAllowed(testedCard));
        }
    }
}
