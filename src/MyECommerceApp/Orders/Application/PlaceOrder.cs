using FluentValidation;
using MassTransit;
using MyECommerceApp.Clients.Infrastructure;
using MyECommerceApp.Orders.Domain;
using MyECommerceApp.ShoppingCart.Infrastructure;
using System.Text.Json.Serialization;

namespace MyECommerceApp.Orders.Application
{
    public static class PlaceOrder
    {
        public class Command
        {
            public Guid ClientId { get; set; }
            public PaymentMethod PaymentMethod { get; set; }
            public DateTimeOffset DeliveryDate { get; set; }
            [JsonIgnore]
            public List<ListShoppingCartItems.Result> ShoppingCartItems { get; set; }
            [JsonIgnore]
            public GetClients.Result Client { get; set; }
        }

        public class Result
        {
            public Guid OrderId { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(command => command.DeliveryDate).GreaterThan(DateTimeOffset.UtcNow).NotEmpty();
            }
        }

        public class Handler
        {
            private readonly IOrderRepository _orderRepository;

            public Handler(IOrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }

            public Task<Result> Handle(Command command)
            {
                var order = new Order(NewId.Next().ToSequentialGuid(),
                    command.PaymentMethod, 
                    command.DeliveryDate, 
                    command.ClientId, 
                    command.Client.Address, 
                    command.ShoppingCartItems.Select(sci=>(sci.ProductId, sci.Name, sci.Price, sci.Quantity)).ToArray());

                _orderRepository.Add(order);

                return Task.FromResult(new Result()
                {
                    OrderId = order.OrderId
                });
            }
        }
    }
}
