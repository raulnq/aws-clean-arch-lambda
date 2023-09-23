using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using FluentValidation;
using MyECommerceApp.Products.Application;
using MyECommerceApp.Products.Infrastructure;
using MyECommerceApp.Shared.Host;
using MyECommerceApp.Shared.Infrastructure.EntityFramework;

namespace MyECommerceApp.Products.Host;

public class Function : BaseFunction
{
    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Post, "/products")]
    public Task<IHttpResult> RegisterProduct(
    [FromServices] AnyProducts.Runner runner,
    [FromServices] TransactionBehavior behavior,
    [FromServices] RegisterProduct.Handler handler,
    [FromBody] RegisterProduct.Command command)
    {
        return Handle(async () =>
        {
            new RegisterProduct.Validator().ValidateAndThrow(command);
            command.Any = await runner.Run(new AnyProducts.Query() { Name = command.Name });
            var result = await behavior.Handle(() => handler.Handle(command));
            return result;
        });
    }

    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Post, "/products/{productId}/enable")]
    public Task<IHttpResult> EnableProduct(
    [FromServices] TransactionBehavior behavior,
    [FromServices] EnableProduct.Handler handler,
    string productId)
    {
        return Handle(async () =>
        {
            var command = new EnableProduct.Command() { ProductId = Guid.Parse(productId) };
            await behavior.Handle(() => handler.Handle(command));
        });
    }
}

