using MyECommerceApp.Shared.Domain;

namespace MyECommerceApp.Shared.Infrastructure.EntityFramework;

public class Repository<T> : IRepository<T>
        where T : class
{
    protected ApplicationDbContext _context;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public void Add(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Remove(T entity)
    {
        _context.Set<T>().Remove(entity);
    }

    public async Task<T> Get(params object[] keyValues)
    {
        var entity = await _context.Set<T>().FindAsync(keyValues);

        if (entity == null)
        {
            throw new NotFoundException<T>();
        }

        return entity;
    }
}
