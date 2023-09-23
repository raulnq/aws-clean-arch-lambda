using MyECommerceApp.Shared.Infrastructure.EntityFramework;
using MyECommerceApp.Shared.Infrastructure.SqlKata;

namespace MyECommerceApp.Products.Infrastructure
{
    public class AnyProducts
    {
        public class Query
        {
            public string Name { get; set; }
        }

        public class Runner : BaseRunner
        {
            public Runner(SqlKataQueryRunner queryRunner) : base(queryRunner) { }

            public Task<bool> Run(Query query)
            {
                return _queryRunner.Any((qf) => {
                    var statement = qf.Query(Tables.Products);

                    if (!string.IsNullOrEmpty(query.Name))
                    {
                        statement = statement.Where(Tables.Products.Field(nameof(Query.Name)), query.Name);
                    }

                    return statement;
                });
            }
        }
    }
}
