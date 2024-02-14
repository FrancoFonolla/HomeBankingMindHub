using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Repositories
{
    public interface ICardRepository
    {
        void Save(Card card);
        IEnumerable<Card> GetCardsByClient(long clienttId);

    }
}
