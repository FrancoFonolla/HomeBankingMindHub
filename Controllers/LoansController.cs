using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HomeBankingMindHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : Controller
    {
        public IClientRepository _clientRepository;
        public IAccountRepository _accountRepository;
        public ILoanRepository _loanRepository;
        public IClientLoanRepository _clientLoanRepository;
        public ITransactionRepository _transactionRepository;

        public LoansController(ITransactionRepository transactionRepository,IClientRepository clientRepository,IClientLoanRepository clientLoanRepository, IAccountRepository accountRepository, ILoanRepository loanRepository)
        {
            _transactionRepository = transactionRepository;
            _clientRepository = clientRepository;
            _clientLoanRepository = clientLoanRepository;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;

        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var loans = _loanRepository.GetAll();
                var loansDTO = new List<LoanDTO>();
                foreach (Loan loan in loans)
                {
                    LoanDTO loanDTO = new LoanDTO
                    {
                        Id = loan.Id,
                        MaxAmount = loan.MaxAmount,
                        Name = loan.Name,
                        Payments = loan.Payments,
                    };
                    loansDTO.Add(loanDTO);
                }
                return Ok(loansDTO);
            }catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Post(LoanApplicationDTO loanApplication)
        {
            try
            {
                //verificamos sin el usuario esta autenticado
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty) return Forbid();
                //verificamos si el cliente existe
                Client client = _clientRepository.FindByEmail(email);
                if (client == null) return Forbid();
                //verificamos sin loan existe
                var loan = _loanRepository.FindById(loanApplication.LoanId);
                if (loan == null) return Forbid();
                //verificamos que amount no se mayor a maxamount ni que sea 0
                if (loanApplication.Amount > loan.MaxAmount || loanApplication.Amount == 0) return Forbid();
                //verificamos que payments no este vacio
                if (loanApplication.Payments == string.Empty) return Forbid();
                //verificamos que la cuenta exista
                var account = _accountRepository.FindByNumber(loanApplication.ToAccountNumber);
                if (account == null) return Forbid();
                //verificamos que la cuenta pertenezca al usuario autenticado
                if (account.Id != client.Id) return Forbid();
                ClientLoan clientLoan = new ClientLoan
                {
                    ClientId = client.Id,
                    LoanId = loanApplication.LoanId,
                    Amount = loanApplication.Amount*0.2,
                    Payments = loanApplication.Payments,

                };
                _clientLoanRepository.Save(clientLoan);
                var transaccion = new Transaction
                {
                    AccountId = account.Id,
                    Amount = loanApplication.Amount,
                    Type = TransactionType.CREDIT.ToString(),
                    Description = "Prestamo",
                    Date = DateTime.Now,
                };
                _transactionRepository.Save(transaccion);
                account.Balance += loanApplication.Amount;
                _accountRepository.Save(account);

                return Created("Con exito", clientLoan);
                

            }catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

    }
}
