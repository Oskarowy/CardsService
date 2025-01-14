using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class Action1PolicyTests
    {
        private readonly Action1Policy _action1policy = new Action1Policy();
        private readonly CardDetails _prepaidActiveCard;

        public Action1PolicyTests()
        {
            _prepaidActiveCard = new CardDetails("123", CardType.Prepaid, CardStatus.Active, true);
        }

        [Fact]
        public void Action1_Allow_ForPrepaidCard_IfCardIsActive()
        {
            Assert.True(_action1policy.IsAllowed(_prepaidActiveCard));
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
