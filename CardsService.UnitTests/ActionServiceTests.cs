using CardsService.Model;
using CardsService.Services;

namespace CardsService.UnitTests
{
    public class ActionServiceTests
    {
        [Fact]
        public void Action1_ShouldBeAllowed_ForPrepaidCard_IfCardIsActive()
        {
            string action1name = "ACTION1";

            var service = new ActionService();
            var cardDetails = new CardDetails("123", CardType.Prepaid, CardStatus.Active, true);

            List<string> allowedActions = service.GetAllowedActions(cardDetails);

            Assert.NotNull(allowedActions);
            Assert.Contains(action1name, allowedActions);
        }
    }
}
