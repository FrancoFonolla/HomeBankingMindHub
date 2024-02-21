using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;
using HomeBankingMindHub.Repositories;

namespace HomeBankingMindHub.Services.Impl
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IAccountRepository _accountRepository;
        public TransactionService(ITransactionRepository transactionRepository, IClientRepository clientRepository, IAccountRepository accountRepository)
        {
            _transactionRepository = transactionRepository;
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
        }
        //metodo de creacion de una transferencias
        public responseClass<Account> Create(TransferDTO transferDTO, string email)
        {
            //verificamos si el email esta vacio
            if (email == string.Empty)
            {
                return new responseClass<Account>(null, "Email vacio", 400);
            }            
            Client client = _clientRepository.FindByEmail(email);
            if (client == null)
                //verificamos si existe el cliente
            {
                return new responseClass<Account>(null, "No existe el cliente", 400);

            }
            //verificamos que las cuentas no esten vacias
            if (transferDTO.FromAccountNumber == string.Empty || transferDTO.ToAccountNumber == string.Empty)
            {
                return new responseClass<Account>(null,"Cuenta origen o de destino no proporcionada",400);
            }
            //verificamos que no se este transfiriendo a la misma cuenta
            if (transferDTO.FromAccountNumber == transferDTO.ToAccountNumber)
            {
                return new responseClass<Account>(null,"No se permite la transferencia a la misma cuenta",403);
            }
            //verificamos que se alla dado una cantidad y una descripcion
            if (transferDTO.Amount == 0 | transferDTO.Description == string.Empty)
            {
                return new responseClass<Account>(null,"Monto o descripcion no proporcionados", 400);
            }
            //que la cantidad no sea negativa
            if(transferDTO.Amount <0)
            {
                return new responseClass<Account>(null, "Monto invalido", 400);
            }
            Account fromAccount = _accountRepository.FindByNumber(transferDTO.FromAccountNumber);
            if (fromAccount.ClientId != client.Id)
            {
                //si la cuenta pertenece al usuario actual
                return new responseClass<Account>(null, "La cuenta de origen no pertenece al cliente autenticado", 403);
            }
            if (fromAccount == null)
            {
                //que exista la cuenta desde la que se quiere transferir
                return new responseClass<Account>(null, "Cuenta origen no existe", 400);
            }
            if (fromAccount.Balance < transferDTO.Amount)
            {
                //la cuenta origen tenga saldo suficiente
                return new responseClass<Account>(null, "Fondos insuficientes", 403);
            }
            Account toAccount = _accountRepository.FindByNumber(transferDTO.ToAccountNumber);
            if (toAccount == null)
            {
                //que exista la cuenta a la que se quiere trasnferir
                return new responseClass<Account>(null, "Cuenta de destino no existe", 403);

            }
            // demas logica para guardado
            //comenzamos con la insercion de las 2 transacciones realizadas
            // desde toAccount se debe generar un debito por lo tanto lo multiplicamos por -1
            _transactionRepository.Save(new Transaction
            {
                Type = TransactionType.DEBIT.ToString(),
                Amount = transferDTO.Amount * -1,
                Description = transferDTO.Description + " " + toAccount.Number,
                AccountId = fromAccount.Id,
                Date = DateTime.Now,
            });
            //ahora un credito para la cuenta fromAccount
            _transactionRepository.Save(new Transaction
            {
                Type = TransactionType.CREDIT.ToString(),
                Amount = transferDTO.Amount,
                Description = transferDTO.Description + " " + fromAccount.Number,
                AccountId = toAccount.Id,
                Date = DateTime.Now,
            });
            //seteamos los valores de las cuentas, a la cuenta de origen le restamos el monto
            fromAccount.Balance = fromAccount.Balance - transferDTO.Amount;
            //actualizamos la cuenta de origen
            _accountRepository.Save(fromAccount);

            //a la cuenta de destino le sumamos el monto
            toAccount.Balance = toAccount.Balance + transferDTO.Amount;
            //actualizamos la cuenta de destino
            _accountRepository.Save(toAccount);

            return new responseClass<Account>(fromAccount, "ok", 200);

        }
    }
}
