using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;

namespace HomeBankingMindHub.Services
{
    public interface ITransactionService
    {
        responseClass<Account> Create(TransferDTO transferDTO, string email);
    }
}
