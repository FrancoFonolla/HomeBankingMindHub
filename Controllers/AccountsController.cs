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

        private IAccountService _accountService;


        public AccountsController(IAccountService accountService)
        {

            _accountService = accountService;
        }


        [HttpGet]
        //endpoint para traer todas las cuentas
        public IActionResult Get()
        {
            try
            {
                var accounts = _accountService.GetAccounts();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id}")]
        //enpoint para traer una cuenta
        public IActionResult Get(long id)
        {
            try
            {
                var account = _accountService.GetAccountById(id);
                if (account.code != 200)
                {
                    return StatusCode(account.code, account.message);
                }
                return Ok(account.Object);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
