using CardsService.Model;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class ActionServiceTests
    {
        [Theory]
        [InlineData(CardStatus.Active)]
        public void Action1_Allow_ForPrepaidCard_IfCardIsActive(CardStatus cardStatus)
        {
            string action1name = "ACTION1";

            var service = new ActionService();
            var cardDetails = new CardDetails("123", CardType.Prepaid, cardStatus, true);

            List<string> allowedActions = service.GetAllowedActions(cardDetails);

            Assert.NotNull(allowedActions);
            Assert.Contains(action1name, allowedActions);
        }

    }
}
