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
        //metodo para crear una cuenta
        public responseClass<Account> CreateAccount(long Id)
        {
            var client= _clientRepository.FindById(Id);
            if (client == null)
            {
                //verificamos que el cliente exista
                return new responseClass<Account>(null, "No existe el cliente", 400);
            }
            if(client.Accounts.Count >= 3) {
                //verificamos que el cliente no tenga mas de  3 cuentas
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
        
        //metodo para traer todas las cuentas
        public IEnumerable<AccountDTO> GetAccounts()
        {
            var accounts = _accountRepository.GetAllAccounts();
            var accountsDTO = new List<AccountDTO>();
            foreach (var account in accounts)
            {
                var newAccountDTO = new AccountDTO(account.Id,account.Number,account.Balance,
                    account.Transactions.Select(tr=> new TransactionDTO(tr.Id,tr.Type,tr.Amount,tr.Description,tr.Date)).ToList(),
                    account.CreationDate);
                accountsDTO.Add(newAccountDTO);
            }
            return accountsDTO;
        }
        //metodo para traer todas las cuentas de un cliente
        public List<AccountDTO> GetAllClientAccounts(long id)
        {
            var accounts = _accountRepository.GetAccountsByClient(id);
            var accountsDTO = new List<AccountDTO>();
            foreach (Account account in accounts)
            {
                AccountDTO accountDTO = new AccountDTO(account.Id, account.Number, account.Balance, account.CreationDate);
                accountsDTO.Add(accountDTO);
            }
            return accountsDTO;
        }
        //metodo para traer una cuenta por Id
        public responseClass<AccountDTO> GetAccountById(long id)
        {
            var account = _accountRepository.FindById(id);
            if (account == null)
                //verificamos que la cuenta exista
                return new responseClass<AccountDTO>(null, "No existe la cuenta", 400);
            var accountDTO = new AccountDTO(account.Id,account.Number,account.Balance, account.Transactions.Select(tr=> new TransactionDTO(tr.Id,tr.Type,tr.Amount,tr.Description,tr.Date)).ToList(),account.CreationDate);
            return new responseClass<AccountDTO>(accountDTO,"ok",200);
        }
        //metodo para verificar que existe una cuenta
        public bool VerifyAccountExist(long id)
        {
            return _accountRepository.FindById(id) == null;
        }
    }
}
