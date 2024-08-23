using Blog.Domain.Core.Data;

namespace Blog.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }
    public int SaveChanges()
    {
        return _context.SaveChanges();
    }
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
