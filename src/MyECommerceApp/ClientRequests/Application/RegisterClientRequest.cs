using FluentValidation;
using MassTransit;
using MyECommerceApp.ClientRequests.Domain;
using MyECommerceApp.Shared.Domain;
using System.Text.Json.Serialization;

namespace MyECommerceApp.ClientRequests.Application;

public static class RegisterClientRequest
{
    public class Command
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public bool Any { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(command => command.Name).MaximumLength(100).NotEmpty();
            RuleFor(command => command.Address).MaximumLength(500).NotEmpty();
            RuleFor(command => command.PhoneNumber).MaximumLength(20).NotEmpty();
        }
    }

    public class Result
    {
        public Guid ClientRequestId { get; set; }
    }

    public class Handler
    {
        private readonly IRepository<ClientRequest> _clientRequestRepository;

        public Handler(IRepository<ClientRequest> clientRequestRepository)
        {
            _clientRequestRepository = clientRequestRepository;
        }

        public Task<Result> Handle(Command command)
        {
            var clientRequest = new ClientRequest (
                NewId.Next().ToSequentialGuid(), 
                command.Name, 
                command.Address, 
                command.PhoneNumber, 
                command.Any);

            _clientRequestRepository.Add(clientRequest);

            return Task.FromResult(new Result()
            {
                ClientRequestId = clientRequest.ClientRequestId
            });
        }
    }
}
