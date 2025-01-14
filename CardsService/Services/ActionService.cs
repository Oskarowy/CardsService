namespace CardsService.Services;

public class ActionService
{
    public ActionService()
    {
       
    }

    public List<string> GetAllowedActions(CardDetails cardDetails) 
    {
        string action1name = "ACTION1";

        return new List<string> { action1name };
    }
}