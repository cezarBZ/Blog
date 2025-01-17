using Blog.Domain.AggregatesModel.UserAggregate;
using System.Linq.Expressions;

namespace Blog.Domain.Core.Data;

public interface IRepository<T, TKey> : IDisposable where T : class
{
    IReadOnlyList<T> GetAll(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
    Task<IReadOnlyList<T>> GetAllAsync(Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null);
    IReadOnlyList<T> Get(Expression<Func<T, bool>> predicate);
    T GetById(TKey id);
    Task<T> GetByIdAsync(TKey id);
    void Add(T entity);
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    IUnitOfWork UnitOfWork { get; }
}
