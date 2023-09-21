using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using FluentValidation;
using MyECommerceApp.ClientRequests.Application;
using MyECommerceApp.ClientRequests.Domain;
using MyECommerceApp.ClientRequests.Infrastructure;
using MyECommerceApp.Shared.Host;
using MyECommerceApp.Shared.Infrastructure.EntityFramework;
using MyECommerceApp.Shared.Infrastructure.Messaging;

namespace MyECommerceApp.ClientRequests.Host;

public class Function : BaseFunction
{
    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Post, "/client-requests")]
    public Task<IHttpResult> RegisterClientRequest(
        [FromServices] AnyClientRequests.Runner runner,
        [FromServices] TransactionBehavior behavior, 
        [FromServices] RegisterClientRequest.Handler handler,
        [FromBody] RegisterClientRequest.Command command)
    {
        return Handle(async ()=>
        {
            new RegisterClientRequest.Validator().ValidateAndThrow(command);
            command.Any = await runner.Run(new AnyClientRequests.Query() { Name = command.Name });
            var result = await behavior.Handle(() => handler.Handle(command));
            return result;
        });
    }

    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Post, "/client-requests/{clientRequestId}/approve")]
    public Task<IHttpResult> ApproveClientRequest(
    [FromServices] TransactionBehavior behavior,
    [FromServices] ApproveClientRequest.Handler handler,
    [FromServices] EventPublisher publisher,
    string clientRequestId)
    {
        return Handle(async () =>
        {
            var command = new ApproveClientRequest.Command() { ClientRequestId = Guid.Parse(clientRequestId) };
            await behavior.Handle(() => handler.Handle(command));
            await publisher.Publish(new ClientRequestApproved(command.ClientRequestId));
        });
    }

    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Get, "/client-requests")]
    public Task<IHttpResult> ListClientRequests(
        [FromServices] ListClientRequests.Runner runner, 
        [FromQuery] string name,
        [FromQuery] int page, 
        [FromQuery] int pageSize)
    {
        return Handle(()=>runner.Run(new ListClientRequests.Query() { Name = name, Page = page, PageSize = pageSize }));
    }
}
