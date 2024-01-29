using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.SQSEvents;
using AWS.Lambda.Powertools.Logging;
using AWS.Lambda.Powertools.Tracing;
using FluentValidation;
using MyECommerceApp.ClientRequests.Domain;
using MyECommerceApp.ClientRequests.Infrastructure;
using MyECommerceApp.Clients.Application;
using MyECommerceApp.Clients.Infrastructure;
using MyECommerceApp.Shared.Host;
using MyECommerceApp.Shared.Infrastructure.EntityFramework;

namespace MyECommerceApp.Clients.Host;

public class Function : BaseFunction
{
    [LambdaFunction]
    [Logging]
    [Tracing]
    public Task<SQSBatchResponse> RegisterClient(
        [FromServices] TransactionBehavior behavior, 
        [FromServices] RegisterClient.Handler handler,
        [FromServices] GetClientRequest.Runner runner,
        SQSEvent sqsEvent)
    {
        return HandleFromSubscription<ClientRequestApproved>(async (clientRequestApproved) =>
        {
            var clientRequest = await runner.Run(new GetClientRequest.Query() { ClientRequestId = clientRequestApproved.ClientRequestId });
            var command = new RegisterClient.Command()
            {
                ClientId = clientRequest.ClientRequestId,
                Address = clientRequest.Address,
                Name = clientRequest.Name,
                PhoneNumber = clientRequest.PhoneNumber,
            };
            new RegisterClient.Validator().ValidateAndThrow(command);
            await behavior.Handle(() => handler.Handle(command));
        }, sqsEvent);
    }

    [LambdaFunction]
    [RestApi(LambdaHttpMethod.Get, "/clients/{clientId}")]
    public Task<IHttpResult> GetClients(
    [FromServices] GetClients.Runner runner,
    string clientId)
    {
        return Handle(() => runner.Run(new GetClients.Query() { ClientId = Guid.Parse(clientId) }));
    }
}

