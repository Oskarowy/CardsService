using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class ActionServiceTests
    {
        private readonly ActionService _actionService = new ActionService();
        private readonly Action1Policy _policy = new Action1Policy();

        [Theory]
        [InlineData(CardStatus.Active)]
        public void Action1_Allow_ForPrepaidCard_IfCardIsActive(CardStatus cardStatus)
        {
            var cardDetails = new CardDetails("123", CardType.Prepaid, cardStatus, true);

            Assert.True(_policy.IsAllowed(cardDetails));
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

            Assert.False(_policy.IsAllowed(cardDetails));
        }

        [Fact]
        public void Action1_NameInPolicy_ShouldBeCorrect()
        {
            Assert.Equal("ACTION1", _policy.ActionName);
        }
    }
}
