using MyECommerceApp.Shared.Infrastructure.EntityFramework;
using MyECommerceApp.Shared.Infrastructure.SqlKata;

namespace MyECommerceApp.Clients.Infrastructure
{
    public class GetClients
    {
        public class Query
        {
            public Guid ClientId { get; set; }
        }

        public class Result
        {
            public Guid ClientId { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string PhoneNumber { get; set; }
        }

        public class Runner : BaseRunner
        {
            public Runner(SqlKataQueryRunner queryRunner) : base(queryRunner) { }

            public Task<Result> Run(Query query)
            {
                return _queryRunner.Get<Result>((qf) => qf
                    .Query(Tables.Clients)
                    .Where(Tables.Clients.Field(nameof(Query.ClientId)), query.ClientId));
            }
        }
    }
}
