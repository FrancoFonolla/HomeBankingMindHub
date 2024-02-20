using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : Controller
    {
        private readonly ITransactionService _transactionService; 
        public TransactionsController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [HttpPost]
        public IActionResult Post([FromBody] TransferDTO transferDTO)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                var fromAccount= _transactionService.Create(transferDTO, email);
                if(fromAccount.code != 200) 
                {
                    return StatusCode(fromAccount.code,fromAccount.message);
                }
                return Created("Creado con exito", fromAccount.Object);
            }
            catch (Exception ex)
            {
            return StatusCode(500, ex.Message);
            }
        }
    }
}
