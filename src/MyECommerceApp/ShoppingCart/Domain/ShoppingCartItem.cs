namespace MyECommerceApp.Clients.Domain;

public class ShoppingCartItem
{
    public Guid ShoppingCartItemId { get; private set; }
    public Guid CLientId { get; private set; }
    public Guid ProductId { get; private set; }
    public decimal Quantity { get; private set; }
}