using HomeBankingMindHub.Models;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace HomeBankingMindHub.Repositories.Implements
{
    public class AccountRepository: RepositoryBase<Account>, IAccountRepository
    {
        Utiles Utils = new Utiles();
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
            bool condition = true;
            string vin = string.Empty;
            while(condition)
            {

                vin = Utils.GeneratedRandomVIN();
                var acc = FindByVIN(vin);
                if (acc ==null)
                {
                    condition = false;
                }
                account.Number = vin;
            }
            Create(account);
            SaveChanges();
        }
        public IEnumerable<Account> GetAccountsByClient(long clientId)
        {
            return FindByCondition(account=> account.ClientId == clientId)
                .Include(account => account.Transactions)
                .ToList();
        }
        public Account FindByVIN(string number)
        {
            return FindByCondition(account => account.Number== number)
                .Include(account=>account.Transactions)
                .FirstOrDefault() ;
        }
    }
}
