using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Repositories;
using HomeBankingMindHub.Services;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : Controller
    {
        
        private readonly ILoansService _loansService;

        public LoansController(ILoansService loansService)
        {
            _loansService = loansService;

        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var loans = _loansService.GetAll();
                return Ok(loans);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post(LoanApplicationDTO loanApplication)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                var clientLoan = _loansService.CreateLoan(loanApplication, email);
                if(clientLoan.code != 200) {
                    return StatusCode(clientLoan.code, clientLoan.message);
                }
                return Created("Con exito", clientLoan.Object);


            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}
