using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using FluentValidation;
using MyECommerceApp.Clients.Infrastructure;
using MyECommerceApp.Orders.Application;
using MyECommerceApp.Orders.Domain;
using MyECommerceApp.Shared.Host;
using MyECommerceApp.Shared.Infrastructure.EntityFramework;
using MyECommerceApp.Shared.Infrastructure.Messaging;
using MyECommerceApp.ShoppingCart.Infrastructure;

namespace MyECommerceApp.Orders.Host;

public class Function : BaseFunction
{
    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Post, "/orders")]
    public Task<IHttpResult> PlaceOrder(
    [FromServices] GetClients.Runner getClientRunner,
    [FromServices] ListShoppingCartItems.Runner listShoppingCartItemsRunner,
    [FromServices] TransactionBehavior behavior,
    [FromServices] PlaceOrder.Handler handler,
    [FromServices] EventPublisher publisher,
    [FromBody] PlaceOrder.Command command)
    {
        return Handle(async () =>
        {
            new PlaceOrder.Validator().ValidateAndThrow(command);
            command.Client = await getClientRunner.Run(new GetClients.Query() { ClientId = command.ClientId });
            command.ShoppingCartItems = await listShoppingCartItemsRunner.Run(new ListShoppingCartItems.Query() { ClientId = command.ClientId });
            var result = await behavior.Handle(() => handler.Handle(command));
            await publisher.Publish(new OrderRegistered(result.OrderId, command.ClientId));
            return result;
        });
    }
}

