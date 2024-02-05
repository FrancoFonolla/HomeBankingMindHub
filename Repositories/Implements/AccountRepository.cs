using HomeBankingMindHub.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeBankingMindHub.Repositories.Implements
{
    public class AccountRepository: RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }
        public Account FindById(long id)
        {
            return FindByCondition(account => account.Id == id)
            .Include(account => account.Transactions)
            .FirstOrDefault();

        }
        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll()
                .Include(Account=> Account.Transactions)
                .ToList();
        }
        public void Save(Account account)
        {
            Create(account);
            SaveChanges();
        }
    }
}
