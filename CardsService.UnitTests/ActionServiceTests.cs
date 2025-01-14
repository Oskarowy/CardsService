using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class ActionServiceTests
    {
        private readonly ActionService _actionService = new ActionService();
        private readonly Action1Policy _action1policy = new Action1Policy();
        private readonly Action2Policy _action2policy = new Action1Policy();

        #region Action1
        [Theory]
        [InlineData(CardStatus.Active)]
        public void Action1_Allow_ForPrepaidCard_IfCardIsActive(CardStatus cardStatus)
        {
            var cardDetails = new CardDetails("123", CardType.Prepaid, cardStatus, true);

            Assert.True(_action1policy.IsAllowed(cardDetails));
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

        [Fact]
        public void ActionNameInPolicy_ShouldBeCorrect()
        {
            Assert.Equal("ACTION1", _action1policy.ActionName);
        }
        #endregion
        #region Action2
        [Theory]
        [InlineData(CardStatus.Inactive)]
        public void Action2_Allow_ForPrepaidCard_IfCardIsNotActive(CardStatus cardStatus)
        {
            var cardDetails = new CardDetails("123", CardType.Prepaid, cardStatus, true);

            Assert.True(_action2policy.IsAllowed(cardDetails));
        }
        #endregion
    }
}
