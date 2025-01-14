using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class Action1PolicyTests
    {
        private readonly Action1Policy _action1policy = new Action1Policy();

        public Action1PolicyTests()
        {
        }

        [Theory]
        [InlineData(CardType.Prepaid)]
        [InlineData(CardType.Debit)]
        [InlineData(CardType.Credit)]
        public void Action1_Allow_ForAnyCardType_IfCardIsActive(CardType cardType)
        {
            var testedCard = new CardDetails("123", cardType, CardStatus.Active, true);
            Assert.True(_action1policy.IsAllowed(testedCard));
        }

        [Theory]
        [InlineData(CardStatus.Ordered)]
        [InlineData(CardStatus.Inactive)]
        [InlineData(CardStatus.Restricted)]
        [InlineData(CardStatus.Blocked)]
        [InlineData(CardStatus.Expired)]
        [InlineData(CardStatus.Closed)]
        public void Action1_Deny_ForPrepaidCard_IfCardIsNotActive(CardStatus cardStatus)
        {
            var cardDetails = new CardDetails("123", CardType.Prepaid, cardStatus, true);

            Assert.False(_action1policy.IsAllowed(cardDetails));
        }      
    }
}
