﻿using MyECommerceApp.Shared.Infrastructure.EntityFramework;
using MyECommerceApp.Shared.Infrastructure.SqlKata;

namespace MyECommerceApp.ClientRequests.Infrastructure;

public class GetClientRequest
{
    public class Query
    {
        public Guid ClientRequestId { get; set; }
    }

    public class Result
    {
        public Guid ClientRequestId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Status { get; set; }
        public DateTimeOffset RegisteredAt { get; set; }
        public DateTimeOffset? ApprovedAt { get; set; }
        public DateTimeOffset? RejectedAt { get; set; }
    }

    public class Runner : BaseRunner
    {
        public Runner(SqlKataQueryRunner queryRunner) : base(queryRunner) { }

        public Task<Result> Run(Query query)
        {
            return _queryRunner.Get<Result>((qf) => qf
                .Query(Tables.ClientRequests)
                .Where(Tables.ClientRequests.Field(nameof(Query.ClientRequestId)), query.ClientRequestId));
        }
    }
}
