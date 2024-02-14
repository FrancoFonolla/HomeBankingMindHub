using HomeBankingMindHub.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Repositories.Implements
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }

        public void Save(Card card)
        {
            Create(card);
            SaveChanges();
        }
        public IEnumerable<Card> GetCardsByClient(long clientId)
        {
            return FindByCondition(card=> card.ClientId == clientId)
            .ToList();
        }
    }
}
