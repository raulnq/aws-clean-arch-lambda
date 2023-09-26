using FluentValidation;
using MassTransit;
using MyECommerceApp.ShoppingCart.Domain;
using System.Text.Json.Serialization;

namespace MyECommerceApp.ShoppingCart.Application;

public static class AddProductToShoppingCart
{
    public class Command
    {
        public Guid ClientId { get; set; }
        public Guid ProductId { get; set; }
        public decimal Quantity { get; set; }
        [JsonIgnore]
        public bool Any { get; set; }
    }

    public class Result
    {
        public Guid ShoppingCartItemId { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(command => command.Quantity).GreaterThan(0);
        }
    }

    public class Handler
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public Handler(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public Task<Result> Handle(Command command)
        {
            var product = new ShoppingCartItem(NewId.Next().ToSequentialGuid(), 
                command.ClientId, 
                command.ProductId, 
                command.Quantity,
                command.Any);

            _shoppingCartRepository.Add(product);

            return Task.FromResult(new Result()
            {
                ShoppingCartItemId = product.ShoppingCartItemId
            });
        }
    }
}
