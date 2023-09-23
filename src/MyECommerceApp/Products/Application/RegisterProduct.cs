using FluentValidation;
using MassTransit;
using MyECommerceApp.Products.Domain;
using MyECommerceApp.Shared.Domain;
using System.Text.Json.Serialization;

namespace MyECommerceApp.Products.Application
{
    public static class RegisterProduct
    {
        public class Command
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            [JsonIgnore]
            public bool Any { get; set; }
        }

        public class Result
        {
            public Guid ProductId { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(command => command.Name).MaximumLength(250).NotEmpty();
                RuleFor(command => command.Description).MaximumLength(500).NotEmpty();
                RuleFor(command => command.Price).GreaterThan(0);
            }
        }

        public class Handler
        {
            private readonly IRepository<Product> _productRepository;

            public Handler(IRepository<Product> productRepository)
            {
                _productRepository = productRepository;
            }

            public Task<Result> Handle(Command command)
            {
                var product = new Product(NewId.Next().ToSequentialGuid(), 
                    command.Name, 
                    command.Description, 
                    command.Price,
                    command.Any);

                _productRepository.Add(product);

                return Task.FromResult(new Result()
                {
                    ProductId = product.ProductId
                });
            }
        }
    }
}
