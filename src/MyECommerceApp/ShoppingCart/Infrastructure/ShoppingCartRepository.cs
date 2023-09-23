using MyECommerceApp.ShoppingCart.Domain;
using MyECommerceApp.Shared.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace MyECommerceApp.ShoppingCart.Infrastructure;

public class ShoppingCartRepository : Repository<ShoppingCartItem>, IShoppingCartRepository
{
    public ShoppingCartRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task Delete(Guid clientId)
    {
        var statement = "DELETE FROM dbo.ShoppingCartItems WHERE ClientId={0}";
        return _context.Database.ExecuteSqlRawAsync(statement, clientId);
    }
}