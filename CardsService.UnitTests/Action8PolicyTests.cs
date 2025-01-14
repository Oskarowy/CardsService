﻿using CardsService.Model;
using CardsService.Policies;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class Action8PolicyTests
    {
        private readonly Action8Policy _action8policy = new Action8Policy();
        private string _cardNumber = "12345";

        public Action8PolicyTests()
        {
        }

        [Theory]
        [MemberData(nameof(CardsMatrixProvider.AllCardsCollection), MemberType = typeof(CardsMatrixProvider))]
        public void Action8_Allow_ForAnyCardType_IfCardIsActiveInActiveOrderedBlocked_NoMatterIfThereIsPIN(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            var availableStatuses = new List<CardStatus> { CardStatus.Active, CardStatus.Inactive, CardStatus.Ordered, CardStatus.Blocked };

            if (!availableStatuses.Contains(cardStatus)) return;

            var testedCard = new CardDetails(_cardNumber, cardType, cardStatus, isPinSet);

            Assert.True(_action8policy.IsAllowed(testedCard));
        }
    }
}
