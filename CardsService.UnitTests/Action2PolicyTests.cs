using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class Action2PolicyTests
    {
        private readonly Action2Policy _action2policy = new Action2Policy();
        private readonly CardDetails _prepaidInactiveCard;

        public Action2PolicyTests()
        {
            _prepaidInactiveCard = new CardDetails("123", CardType.Prepaid, CardStatus.Inactive, true);
        }

        [Fact]
        public void Action2_Allow_ForPrepaidCard_IfCardIsInactive()
        {
            Assert.True(_action2policy.IsAllowed(_prepaidInactiveCard));
        }

        [Theory]
        [InlineData(CardStatus.Ordered)]
        [InlineData(CardStatus.Active)]
        [InlineData(CardStatus.Restricted)]
        [InlineData(CardStatus.Blocked)]
        [InlineData(CardStatus.Expired)]
        [InlineData(CardStatus.Closed)]
        public void Action2_Deny_ForPrepaidCard_IfCardIsNotInactive(CardStatus cardStatus)
        {
            var cardDetails = new CardDetails("123", CardType.Prepaid, cardStatus, true);

            Assert.False(_action2policy.IsAllowed(cardDetails));
        }
    }
}
