using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Mvc;
using NuGet.LibraryModel;
using NuGet.Protocol;


namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class AccountsController : Controller
    {
        private IAccountRepository _accountRepository;
        private IClientRepository _clientRepository;
        Utiles Utils= new Utiles();

        public AccountsController(IAccountRepository accountRepository, IClientRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
        }

        
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var accounts= _accountRepository.GetAllAccounts();

                var accountsDTO= new List<AccountDTO>();
                foreach (var account in accounts)
                {
                    var newAccountDTO = new AccountDTO
                    {
                        Id = account.Id,
                        Balance = account.Balance,
                        CreationDate = account.CreationDate,
                        Number = account.Number,
                        Transactions = account.Transactions.Select(tr => new TransactionDTO { Id = tr.Id,
                            Amount = tr.Amount,
                            Date = tr.Date,
                            Description = tr.Description,
                            Type=tr.Type}).ToList()
                    };
                    accountsDTO.Add(newAccountDTO);
                }
                return Ok(accountsDTO);
            }catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var account = _accountRepository.FindById(id);
                if(account == null)
                {
                    return Forbid();
                }
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
                return Ok(accountDTO);
                
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);

            }
        }
        //[Route("clients/current/accounts")]
        //[HttpPost]
        //public IActionResult Post()
        //{
        //    try
        //    {
        //        string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
        //        if(email == string.Empty)
        //        {
        //            return Forbid();
        //        }
        //        Client client= _clientRepository.FindByEmail(email);
        //        if(client == null)
        //        {
        //            return NotFound();
        //        }
        //        long id = client.Id;
        //        if(client.Accounts.Count>=3)
        //        {
        //            return Forbid();
        //        }
                
        //        var newAccount = new Account
                
        //        {
        //            Balance = 0,
        //            CreationDate = DateTime.Now,
        //            Transactions= new List<Transaction>(),
        //            ClientId = id,
        //        };
        //        _accountRepository.Save(newAccount);
        //        return Created("", newAccount);
                
        //    }catch (Exception ex)
        //    {
        //        return StatusCode(500,ex.Message);
        //    }
        //}
    }
}
