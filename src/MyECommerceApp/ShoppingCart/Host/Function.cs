using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using FluentValidation;
using MyECommerceApp.ShoppingCart.Application;
using MyECommerceApp.ShoppingCart.Infrastructure;
using MyECommerceApp.Shared.Host;
using MyECommerceApp.Shared.Infrastructure.EntityFramework;
using Amazon.Lambda.SQSEvents;
using MyECommerceApp.Orders.Domain;
using MyECommerceApp.ShoppingCart.Domain;

namespace MyECommerceApp.ShoppingCart.Host;

public class Function : BaseFunction
{
    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Post, "/shopping-cart")]
    public Task<IHttpResult> AddProductToShoppingCart(
    [FromServices] AnyShoppingCartItems.Runner runner,
    [FromServices] TransactionBehavior behavior,
    [FromServices] AddProductToShoppingCart.Handler handler,
    [FromBody] AddProductToShoppingCart.Command command)
    {
        return Handle(async () =>
        {
            new AddProductToShoppingCart.Validator().ValidateAndThrow(command);
            command.Any = await runner.Run(new AnyShoppingCartItems.Query() { ClientId = command.ClientId, ProductId = command.ProductId });
            var result = await behavior.Handle(() => handler.Handle(command));
            return result;
        });
    }

    [LambdaFunction]
    public Task<SQSBatchResponse> CleanShoppingCart(
    [FromServices] TransactionBehavior behavior,
    [FromServices] IShoppingCartRepository repository,
    SQSEvent sqsEvent)
    {
        return HandleFromSubscription<OrderRegistered>(async (orderRegistered) =>
        {
            await behavior.Handle(() => repository.Delete(orderRegistered.ClientId));
        }, sqsEvent);
    }
}

