namespace MyECommerceApp.Shared.Domain;

public interface IRepository<T> where T : class
{
    Task<T> Get(params object[] keyValues);

    void Add(T entity);

    void Remove(T entity);
}
