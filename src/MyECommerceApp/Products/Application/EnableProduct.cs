using MyECommerceApp.Products.Domain;
using MyECommerceApp.Shared.Domain;

namespace MyECommerceApp.Products.Application
{
    public static class EnableProduct
    {
        public class Command
        {
            public Guid ProductId { get; set; }
        }

        public class Handler
        {
            private readonly IRepository<Product> _productRepository;

            public Handler(IRepository<Product> productRepository)
            {
                _productRepository = productRepository;
            }

            public async Task Handle(Command command)
            {
                var clientRequest = await _productRepository.Get(command.ProductId);

                clientRequest.Enable();
            }
        }
    }
}
