using Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Tests.Integration.Setup;

public class BaseIntegrationTest: IClassFixture<TestDatabaseFactory>, IDisposable
{
    protected readonly IServiceScope _scope;
    protected readonly ISender _sender;
    protected readonly MainDbContext _context;
    public BaseIntegrationTest(TestDatabaseFactory factory)
    {
        _scope = factory.Services.CreateScope();
        _sender = _scope.ServiceProvider.GetRequiredService<ISender>();
        _context = _scope.ServiceProvider.GetRequiredService<MainDbContext>();

        _context.Database.BeginTransaction();
    }

    public void Dispose()
    {
        _context.Database.RollbackTransaction();
    }
}