using Amazon.Lambda.Annotations;
using Amazon.Lambda.SQSEvents;
using FluentValidation;
using MyECommerceApp.ClientRequests.Domain;
using MyECommerceApp.ClientRequests.Infrastructure;
using MyECommerceApp.Clients.Application;
using MyECommerceApp.Shared.Host;
using MyECommerceApp.Shared.Infrastructure.EntityFramework;

namespace MyECommerceApp.Clients.Host;

public class Function : BaseFunction
{
    [LambdaFunction]
    public Task<SQSBatchResponse> RegisterClient(
        [FromServices] TransactionBehavior behavior, 
        [FromServices] RegisterClient.CommandHandler handler,
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
}

