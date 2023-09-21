namespace MyECommerceApp.Clients.Domain;

public class Products
{
    public Guid ProductId { get; private set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public bool IsEnabled { get; set; }
}