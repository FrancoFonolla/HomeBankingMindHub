using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Repositories
{
    public interface ILoanRepository
    {
        ICollection<Loan> GetAll();
        Loan FindById(long id);
    }
}
