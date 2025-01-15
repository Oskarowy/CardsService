using System.Runtime.Serialization;

namespace CardsService.Model
{
    public record CardRequest(string UserId, string CardNumber);
}
