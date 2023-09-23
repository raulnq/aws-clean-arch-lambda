using MyECommerceApp.Shared.Domain;

namespace MyECommerceApp.ShoppingCart.Domain;

public interface IShoppingCartRepository : IRepository<ShoppingCartItem>
{
    Task Delete(Guid clientId);
}