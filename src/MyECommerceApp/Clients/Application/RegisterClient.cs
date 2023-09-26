using FluentValidation;
using MyECommerceApp.Clients.Domain;
using MyECommerceApp.Shared.Domain;

namespace MyECommerceApp.Clients.Application;

public static class RegisterClient
{
    public class Command
    {
        public Guid ClientId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class Result
    {
        public Guid ClientId { get; set; }
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

    public class Handler
    {
        private readonly IRepository<Client> _clientRepository;

        public Handler(IRepository<Client> clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public Task<Result> Handle(Command command)
        {
            var clientRequest = new Client(command.ClientId, command.Name, command.Address, command.PhoneNumber);

            _clientRepository.Add(clientRequest);

            return Task.FromResult(new Result()
            {
                ClientId = clientRequest.ClientId
            });
        }
    }
}
