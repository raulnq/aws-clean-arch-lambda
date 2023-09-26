using MyECommerceApp.Shared.Domain;

namespace MyECommerceApp.Orders.Domain;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order> Get(Guid orderId);
}
