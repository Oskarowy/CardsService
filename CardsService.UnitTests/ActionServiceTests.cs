using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class ActionServiceTests
    {
        private readonly ActionService _actionService = new ActionService();
        private readonly Action1Policy _action1policy = new Action1Policy();
        private readonly Action2Policy _action2policy = new Action2Policy();
        private readonly CardDetails _prepaidActiveCard;
        private readonly CardDetails _prepaidInactiveCard;

        public ActionServiceTests()
        {
            _prepaidActiveCard = new CardDetails("123", CardType.Prepaid, CardStatus.Active, true);
            _prepaidInactiveCard = new CardDetails("123", CardType.Prepaid, CardStatus.Inactive, true);
        }

        #region Action1
        [Fact]
        public void Action1_Allow_ForPrepaidCard_IfCardIsActive()
        {
            Assert.True(_action1policy.IsAllowed(_prepaidActiveCard));
        }

        [Fact]
        public void Action1_ShouldBeReturnedByService_IfPolicyIsAllowed()
        {
            var allowedActions = _actionService.GetAllowedActions(_prepaidActiveCard);

            Assert.True(_action1policy.IsAllowed(_prepaidActiveCard));
            Assert.Contains(_action1policy.ActionName, allowedActions);
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
        #endregion
        #region Action2
        [Fact]
        public void Action2_Allow_ForPrepaidCard_IfCardIsInactive()
        {
            Assert.True(_action2policy.IsAllowed(_prepaidInactiveCard));
        }

        [Fact]
        public void Action2_ShouldBeReturnedByService_IfPolicyIsAllowed()
        {
            var allowedActions = _actionService.GetAllowedActions(_prepaidActiveCard);

            Assert.True(_action2policy.IsAllowed(_prepaidInactiveCard));
            Assert.Contains(_action2policy.ActionName, allowedActions);
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
        #endregion
    }
}
