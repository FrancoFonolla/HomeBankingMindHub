using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Repositories;

namespace HomeBankingMindHub.Services.Impl
{
    public class LoansService : ILoansService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IAccountRepository _accountRepository;
        private readonly IClientLoanRepository _clientLoanRepository;
        private readonly ILoanRepository _loanRepository;

        public LoansService(IClientLoanRepository clientLoanRepository,IClientRepository clientRepository,IAccountRepository accountRepository,ITransactionRepository transactionRepository,ILoanRepository loanRepository)
        {
            _clientRepository = clientRepository;
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _loanRepository = loanRepository;
            _clientLoanRepository = clientLoanRepository;
        }
        
        public responseClass<ClientLoan> CreateLoan(LoanApplicationDTO loanApplication, string email)
        {
            if (email == string.Empty|| !Utiles.IsValidEmail(email)) return new responseClass<ClientLoan>(null,"Email invalido o vacio",400);
            //verificamos si el cliente existe
            Client client = _clientRepository.FindByEmail(email);
            if (client == null) return new responseClass<ClientLoan>(null,"El cliente no existe",400);
            //verificamos si loan existe
            var loan = _loanRepository.FindById(loanApplication.LoanId);
            if (loan == null) return new responseClass<ClientLoan>(null,"No existe el tipo de prestamo",400);
            //verificamos que amount no se mayor a maxamount ni que sea 0 o menor a 0
            if (loanApplication.Amount > loan.MaxAmount || loanApplication.Amount <= 0 || loanApplication.Amount == null) return new responseClass<ClientLoan>(null, "Monto invalido",400);
            //verificamos que payments no este vacio
            if (loanApplication.Payments == string.Empty) return new responseClass<ClientLoan>(null,"El campo payments no puede estar vacio", 400);
            //verificamos que la cantidad de pagos sea correcta
            string[] paymentss = loan.Payments.Split(',');
            bool prueba = false;
            foreach (string payment in paymentss)
            {
                if (payment == loanApplication.Payments)
                {
                    prueba = true;
                }
            }
            if (!prueba) return new responseClass<ClientLoan>(null, "Cantidad de pagos invalida", 400);
                


            //verificamos que la cuenta exista
            var account = _accountRepository.FindByNumber(loanApplication.ToAccountNumber);
            if (account == null) return new responseClass<ClientLoan>(null,"Cuenta  no existe", 400);
            //verificamos que la cuenta pertenezca al usuario autenticado
            if (account.Id != client.Id) return new responseClass<ClientLoan>(null, "La cuenta no pertenece al cliente actual", 400);
            ClientLoan clientLoan = new ClientLoan
            {
                ClientId = client.Id,
                LoanId = loanApplication.LoanId,
                Amount = loanApplication.Amount * 0.2,
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
            return new responseClass<ClientLoan>(clientLoan, "ok", 200);
        }

        public List<LoanDTO> GetAll()
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
            return loansDTO;
        }
    }
}
