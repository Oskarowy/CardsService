namespace CardsService.Services;

public class ActionService
{
    public ActionService()
    {
       
    }

    public List<string> GetAllowedActions(CardDetails cardDetails) 
    {
        string action1name = "ACTION1";
        var result = new List<string>();

        if (cardDetails.CardStatus == Model.CardStatus.Active)
            result.Add(action1name);

        return result;
    }
}