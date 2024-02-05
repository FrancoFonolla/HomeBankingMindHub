using Microsoft.EntityFrameworkCore.Query;

namespace HomeBankingMindHub.Repositories
{
    public interface IRepositoryBase<T>
    {
        IQueryable<T> FindAll();
        IQueryable<T> FindAll(Func<IQueryable<T>,IIncludableQueryable<T,object>>includes=null);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
