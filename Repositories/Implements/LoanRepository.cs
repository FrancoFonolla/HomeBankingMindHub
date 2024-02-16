using HomeBankingMindHub.Models;

namespace HomeBankingMindHub.Repositories.Implements
{
    public class LoanRepository : RepositoryBase<Loan>, ILoanRepository
    {
        public LoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }
        public ICollection<Loan> GetAll()
        {
            return FindAll().ToList();
        }
        public Loan FindById(long id) {
            return FindByCondition(loan => loan.Id == id).FirstOrDefault();
        }
    }
}
