using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Repositories;

namespace HomeBankingMindHub.Services.Impl
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _accountRepository;
        private IClientRepository _clientRepository;
        
        public AccountService(IAccountRepository accountRepository,IClientRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
        }
        public responseClass<Account> CreateAccount(long Id)
        {
            var client= _clientRepository.FindById(Id);
            if (client == null)
            {
                return new responseClass<Account>(null, "No existe el cliente", 400);
            }
            if(client.Accounts.Count >= 3) {
                return new responseClass<Account>(null, "El cliente ya tiene 3 cuentas", 401);
            }
            Account account = new Account();
            account.Balance = 0;
            account.CreationDate = DateTime.Now;
            account.Transactions = new List<Transaction>();
            account.ClientId = Id;
            _accountRepository.Save(account);
            
            return new responseClass<Account>(account,"ok",200);
        }
        
        public IEnumerable<AccountDTO> GetAccounts()
        {
            var accounts = _accountRepository.GetAllAccounts();
            var accountsDTO = new List<AccountDTO>();
            foreach (var account in accounts)
            {
                var newAccountDTO = new AccountDTO
                {
                    Id = account.Id,
                    Balance = account.Balance,
                    CreationDate = account.CreationDate,
                    Number = account.Number,
                    Transactions = account.Transactions.Select(tr => new TransactionDTO
                    {
                        Id = tr.Id,
                        Amount = tr.Amount,
                        Date = tr.Date,
                        Description = tr.Description,
                        Type = tr.Type
                    }).ToList()
                };
                accountsDTO.Add(newAccountDTO);
            }
            return accountsDTO;
        }
        public List<AccountDTO> GetAllClientAccounts(long id)
        {
            var accounts = _accountRepository.GetAccountsByClient(id);
            var accountsDTO = new List<AccountDTO>();
            foreach (Account account in accounts)
            {
                AccountDTO accountDTO = new AccountDTO
                {
                    Balance = account.Balance,
                    Number = account.Number,
                    CreationDate = account.CreationDate,
                    Id = id,

                };
                accountsDTO.Add(accountDTO);
            }
            return accountsDTO;
        }
        public responseClass<AccountDTO> GetAccountById(long id)
        {
            var account = _accountRepository.FindById(id);
            if (account == null)
                return new responseClass<AccountDTO>(null, "No existe la cuenta", 400);
            var accountDTO = new AccountDTO
            {
                Id = account.Id,
                Number = account.Number,
                Balance = account.Balance,
                CreationDate = account.CreationDate,
                Transactions = account.Transactions.Select(tr => new TransactionDTO
                {
                    Id = tr.Id,
                    Type = tr.Type,
                    Amount = tr.Amount,
                    Description = tr.Description,
                    Date = tr.Date
                }).ToList()
            };
            return new responseClass<AccountDTO>(accountDTO,"ok",200);
        }
        public bool VerifyAccountExist(long id)
        {
            return _accountRepository.FindById(id) == null;
        }
    }
}
