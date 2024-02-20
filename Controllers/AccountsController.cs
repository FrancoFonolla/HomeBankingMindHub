using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Services;
using Microsoft.AspNetCore.Mvc;
using NuGet.LibraryModel;
using NuGet.Protocol;


namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class AccountsController : Controller
    {
        
        private IAccountService   _accountService;
        

        public AccountsController(IAccountService accountService)
        {
            
            _accountService = accountService;
        }

        
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var accounts= _accountService.GetAccounts();
                return Ok(accounts);
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
                var account = _accountService.GetAccountById(id);
                if(account.code != 200)
                {
                    return StatusCode(account.code,account.message);
                }
               
                return Ok(account.Object);
                
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
