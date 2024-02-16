using HomeBankingMindHub.Models;
using Transaction = HomeBankingMindHub.Models.Transaction;

namespace HomeBankingMindHub.Repositories.Implements
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(HomeBankingContext repositoryContext) : base(repositoryContext)
        {
        }
        public Transaction FindByNumber(long id)
        {
            return FindByCondition(transaction => transaction.Id == id).FirstOrDefault();

        }
        public void Save(Transaction transaction)
        {
            Create(transaction);
            SaveChanges();
        }
    }
}
