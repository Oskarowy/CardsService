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
            var allowedActions = _actionService.GetAllowedActions(cardDetails);

            Assert.True(_policy.IsAllowed(cardDetails));
            Assert.Contains("ACTION1", allowedActions);
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
            var allowedActions = _actionService.GetAllowedActions(cardDetails);

            Assert.False(_policy.IsAllowed(cardDetails));
            Assert.DoesNotContain("ACTION1", allowedActions);
        }
    }
}
