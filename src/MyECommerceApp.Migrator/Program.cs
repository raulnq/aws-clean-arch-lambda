using Microsoft.Data.SqlClient;
using Polly;

namespace MyECommerceApp.Migrator;

public class Program
{
    static void Main(string[] args)
    {
        var migrator = new DbMigrator(typeof(Program).Assembly);

        Policy
            .Handle<SqlException>(exception => exception.Message.Contains("pre-login handshake"))
            .WaitAndRetry(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            })
            .Execute(() => migrator.Migrate());
    }
}
