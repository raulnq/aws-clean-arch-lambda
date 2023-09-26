using MyECommerceApp.ClientRequests.Domain;
using MyECommerceApp.Shared.Domain;

namespace MyECommerceApp.ClientRequests.Application;

public static class ApproveClientRequest
{
    public class Command
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

        public async Task Handle(Command command)
        {
            var clientRequest = await _clientRequestRepository.Get(command.ClientRequestId);

            clientRequest.Approve();
        }
    }
}
