using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text.Json.Serialization;
using System.Text.Json;
using MyECommerceApp.ClientRequests.Application;
using MyECommerceApp.Shared.Infrastructure.ExceptionHandling;
using System.Web;
using Shouldly;
using System.Diagnostics;
using Xunit;
using Bogus;

namespace MyECommerceApp.Tests.ClientRequests;

public class ApiGatewayUrlProvider
{
    private static readonly Lazy<string> _lazy = new Lazy<string>(() => Build());

    public readonly static string Url = _lazy.Value;

    private static string Build()
    {
        var startInfo = new ProcessStartInfo()
        {
            FileName = "aws", 
            Arguments = "apigateway get-rest-apis --endpoint-url=http://localhost:4566 --query \"items[0].id\"",
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            UseShellExecute = false,
        };

        var process = Process.Start(startInfo);

        if(process is null)
        {
            throw new ApplicationException("Missing Gateway URL");
        }

        var output = process.StandardOutput.ReadLine();

        if (string.IsNullOrEmpty(output))
        {
            throw new ApplicationException("Missing Gateway URL");
        }

        var apiGatewayId = output.Replace("\"", "");

        if(string.IsNullOrEmpty(apiGatewayId) || apiGatewayId.Contains("null"))
        {
            throw new ApplicationException("Missing Gateway URL");
        }

        return $"http://{apiGatewayId}.execute-api.localhost.localstack.cloud:4566";
    }
}

public class RegisterClientRequestTests : IAsyncLifetime
{
    private readonly AppDsl _appDsl;

    public RegisterClientRequestTests()
    {
        _appDsl = new AppDsl();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _appDsl.DisposeAsync().AsTask();
    }


    [Fact]
    public Task register_should_be_ok()
    {
        return _appDsl.ClientRequests.Register();
    }
}

public class ApproveClientRequestTests : IAsyncLifetime
{
    private readonly AppDsl _appDsl;

    public ApproveClientRequestTests()
    {
        _appDsl = new AppDsl();
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return _appDsl.DisposeAsync().AsTask();
    }


    [Fact]
    public async Task approve_should_be_ok()
    {
        var (_, result) = await _appDsl.ClientRequests.Register();

        await _appDsl.ClientRequests.Approve(c => c.ClientRequestId = result.ClientRequestId);
    }
}


public class AppDsl : IAsyncDisposable
{
    private readonly HttpClient _client;

    public AppDsl()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri(ApiGatewayUrlProvider.Url)
        };

        ClientRequests = new ClientRequestsDsl(_client);
    }

    public ClientRequestsDsl ClientRequests { get; }

    public ValueTask DisposeAsync()
    {
        _client.Dispose();

        return ValueTask.CompletedTask;
    }
}


public class ClientRequestsDsl : Dsl
{
    private readonly HttpClient _httpClient;
    private readonly string _path = "Prod/api/client-requests";

    public ClientRequestsDsl(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<(RegisterClientRequest.Command, RegisterClientRequest.Result?)> Register(Action<RegisterClientRequest.Command>? setup = null, string? error = null)
    {
        var faker = new Faker<RegisterClientRequest.Command>()
            .RuleFor(command => command.Name, faker => faker.Random.Guid().ToString());

        var request = faker.Generate();

        setup?.Invoke(request);

        var result = await _httpClient.Post<RegisterClientRequest.Command, RegisterClientRequest.Result>(_path, request);

        result.Check(error, successAssert: result =>
        {
            result.ClientRequestId.ShouldNotBe(Guid.Empty);
        });

        return (request, result.Response);
    }

    public async Task<ApproveClientRequest.Command> Approve(Action<ApproveClientRequest.Command>? setup = null, string? error = null)
    {
        var request = new ApproveClientRequest.Command();

        setup?.Invoke(request);

        var result = await _httpClient.Post<ApproveClientRequest.Command, EmptyResponse>($"{_path}/{request.ClientRequestId}/approve", request);

        result.Check(error);

        return request;
    }    
}

public static class HttpExtensions
{
    public static async Task<Result<TResponse>> Post<TRequest, TResponse>(this HttpClient client, string requestUri, TRequest request)
        where TResponse : class
    {
        var requestbody = JsonSerializer.Serialize(request);

        var httpResponse = await client.PostAsync(requestUri, new StringContent(requestbody, Encoding.Default, "application/json"));

        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        options.Converters.Add(new JsonStringEnumConverter());

        var responseBody = await httpResponse.Content.ReadAsStringAsync();

        if (httpResponse.IsSuccessStatusCode)
        {
            var response = JsonSerializer.Deserialize<TResponse>(responseBody, options);

            return new Result<TResponse>(){ Response = response };
        }
        else
        {
            var error = JsonSerializer.Deserialize<ProblemDetails>(responseBody, options);

            return new Result<TResponse>(){ Error = error };
        }
    }

    public static async Task<Result<TResponse>> Get<TResponse>(this HttpClient client, string requestUri)
        where TResponse : class
    {
        var httpResponse = await client.GetAsync(requestUri);

        var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

        options.Converters.Add(new JsonStringEnumConverter());

        var responseBody = await httpResponse.Content.ReadAsStringAsync();

        if (httpResponse.IsSuccessStatusCode)
        {
            var response = JsonSerializer.Deserialize<TResponse>(responseBody, options);

            return new Result<TResponse>(){ Response = response };
        }
        else
        {
            var error = JsonSerializer.Deserialize<ProblemDetails>(responseBody, options);

            return new Result<TResponse>(){ Error = error };
        }
    }

    public static Task<Result<TResponse>> Get<TRequest, TResponse>(this HttpClient client, string requestUri, TRequest request)
        where TResponse : class
    {
        var uriBuilder = new UriBuilder($"host/{requestUri.TrimStart('/')}");

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        foreach (var param in typeof(TRequest).GetProperties())
        {
            var value = param.GetValue(request)?.ToString();
            if (!string.IsNullOrEmpty(value))
            {
                query[param.Name.ToLower()] = value;
            }
        }

        uriBuilder.Query = query.ToString();

        return client.Get<TResponse>(uriBuilder.Uri.PathAndQuery);
    }
}

public record EmptyRequest();

public record EmptyResponse();

public class Result<TResponse> 
    where TResponse : class
{
    public TResponse Response { get; set; }   
    public ProblemDetails Error { get; set; }

    public void ShouldBeSuccessful()
    {
        Error.ShouldBeNull();
        Response.ShouldNotBeNull();
    }

    public void ShouldThrowError(string errorDetail)
    {
        Error.ShouldNotBeNull();
        Error.Detail.ShouldBe(errorDetail);
        Response.ShouldBeNull();
    }

    public void Check(string? errorDetail = null, Action<TResponse>? successAssert = null, Action<ProblemDetails>? errorAssert = null)
    {
        if (errorDetail == null)
        {
            ShouldBeSuccessful();
            successAssert?.Invoke(Response!);
        }
        else
        {
            ShouldThrowError(errorDetail);
            errorAssert?.Invoke(Error!);

        }
    }
}

public abstract class Dsl
{
    protected readonly TimeoutMonitor _timeoutMonitor;
    protected readonly static TimeSpan _defaultTimeout = TimeSpan.FromSeconds(30);
    protected readonly static TimeSpan _defaultInterval = TimeSpan.FromSeconds(30);
    protected Dsl()
    {
        _timeoutMonitor = new TimeoutMonitor();
    }

    public Task WaitFor(Func<Task> taskFactory, TimeSpan timeout, TimeSpan interval) => _timeoutMonitor.RunUntil(taskFactory, timeout, interval);
}
