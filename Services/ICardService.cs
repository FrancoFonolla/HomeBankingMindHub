using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;

namespace HomeBankingMindHub.Services
{
    public interface ICardService
    {
        responseClass<Card> CreateCard(CardDTO CardFront, Client client);
        List<CardDTO> GetCards(IEnumerable<Card> cards);

    }
}
