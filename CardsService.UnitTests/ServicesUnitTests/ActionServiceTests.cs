﻿using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;
using CardsService.Tests.Tools;

namespace CardsService.Tests.ServicesUnitTests
{
    public class ActionServiceTests
    {
        private readonly ActionService _actionService;
        private readonly List<IActionPolicy> _policies;
        private readonly PoliciesImplementationProvider policiesImplementationProvider = new PoliciesImplementationProvider();
        private string _cardNumber = "12345";

        public ActionServiceTests()
        {
            _policies = policiesImplementationProvider.Implementations.ToList();
            _actionService = new ActionService(_policies);
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action_ShouldBeReturnedByService_IfPolicyIsAllowed(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            var allowedActions = _actionService.GetAllowedActions(testedCard);

            foreach (var policy in _policies)
            {
                var isPolicyAllowed = policy.IsAllowed(testedCard);

                if (isPolicyAllowed)
                {
                    Assert.Contains(policy.ActionName, allowedActions);
                    Assert.StartsWith(policy.ActionName.ToLower(), policy.GetType().Name.ToLower());
                }
                else
                    Assert.False(isPolicyAllowed);
            }
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action_ShouldBeDeniedByService_IfPolicyIsNotAllowed(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            var allowedActions = _actionService.GetAllowedActions(testedCard);

            foreach (var policy in _policies)
            {
                var isPolicyAllowed = policy.IsAllowed(testedCard);

                if (!isPolicyAllowed)
                    Assert.DoesNotContain(policy.ActionName, allowedActions);
                else
                    Assert.True(isPolicyAllowed);
            }
        }
    }
}
