using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;

namespace HomeBankingMindHub.Services
{
    public interface IAccountService
    {
        responseClass<Account> CreateAccount(long id);
        

        IEnumerable<AccountDTO> GetAccounts();
        List<AccountDTO> GetAllClientAccounts(long id);
        responseClass<AccountDTO> GetAccountById(long id);
        bool VerifyAccountExist(long id);

    }
}
