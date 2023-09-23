using Microsoft.EntityFrameworkCore;
using MyECommerceApp.Orders.Domain;
using MyECommerceApp.Shared.Infrastructure.EntityFramework;

namespace MyECommerceApp.Orders.Infrastructure;

public class OrderRepository : Repository<Order>, IOrderRepository
{
    public OrderRepository(ApplicationDbContext context):base(context)
    {

    }

    public async Task<Order> Get(Guid orderId)
    {
        var order = await _context.Set<Order>().FindAsync(orderId);

        if(order?.Items?.Count>0)
        {
            return order;
        }

        order = _context.Set<Order>().Include(o => o.Items).First(o => o.OrderId == orderId);

        return order;
    }
}