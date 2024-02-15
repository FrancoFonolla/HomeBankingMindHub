using System.Transactions;
using Transaction = HomeBankingMindHub.Models.Transaction;
namespace HomeBankingMindHub.Repositories
{
    public interface ITransactionRepository
    {
        void Save(Transaction transaction);
        Transaction FindByNumber(long id);

    }
}
