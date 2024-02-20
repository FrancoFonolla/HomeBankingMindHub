using HomeBankingMindHub.Models;
using HomeBankingMindHub.Models.DTOs;

namespace HomeBankingMindHub.Services
{
    public interface ILoansService
    {
        List<LoanDTO> GetAll();
        responseClass<ClientLoan> CreateLoan(LoanApplicationDTO loanApplication, string email);

    }
}
