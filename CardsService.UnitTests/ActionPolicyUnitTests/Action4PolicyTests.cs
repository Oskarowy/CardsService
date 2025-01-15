using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;
using CardsService.Tests.Tools;

namespace CardsService.Tests.ActionPolicyUnitTests
{
    public class Action4PolicyTests
    {
        private readonly Action4Policy _action4policy = new Action4Policy();
        private string _cardNumber = "12345";

        public Action4PolicyTests()
        {
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action4_Allow_ForAnyCardType_ForAnyCardStatus_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.True(_action4policy.IsAllowed(testedCard));
        }
    }
}
