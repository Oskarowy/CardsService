using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class ActionServiceTests
    {
        private readonly ActionService _actionService;
        private readonly List<IActionPolicy> _policies;
        private readonly Action1Policy _action1policy = new Action1Policy();
        private readonly Action2Policy _action2policy = new Action2Policy();
        private readonly CardDetails _prepaidActiveCard;
        private readonly CardDetails _prepaidInactiveCard;

        public ActionServiceTests()
        {
            _policies = new List<IActionPolicy> 
            {
                _action1policy,_action2policy
            };
            _actionService = new ActionService(_policies);
            _prepaidActiveCard = new CardDetails("123", CardType.Prepaid, CardStatus.Active, true);
            _prepaidInactiveCard = new CardDetails("123", CardType.Prepaid, CardStatus.Inactive, true);
        }

        [Fact]
        public void Action1_ShouldBeReturnedByService_IfPolicyIsAllowed()
        {
            var allowedActions = _actionService.GetAllowedActions(_prepaidActiveCard);

            Assert.True(_action1policy.IsAllowed(_prepaidActiveCard));
            Assert.Contains(_action1policy.ActionName, allowedActions);
        }

        [Fact]
        public void Action2_ShouldBeReturnedByService_IfPolicyIsAllowed()
        {
            var allowedActions = _actionService.GetAllowedActions(_prepaidInactiveCard);

            Assert.True(_action2policy.IsAllowed(_prepaidInactiveCard));
            Assert.Contains(_action2policy.ActionName, allowedActions);
        }
    }
}
