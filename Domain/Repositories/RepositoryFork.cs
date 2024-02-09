using Microsoft.AspNetCore.Http.HttpResults;

namespace Domain.Repositories;

public class RepositoryFork
{
    public Guid ForkedRepoId { get; private set; }
    public Repository? ForkedRepo { get; private set; }
    public Guid SourceRepoId { get; private  set; }
    public Repository? SourceRepo { get; private set; }
    
    private RepositoryFork()
    {
    }

    private RepositoryFork(Repository sourceRepo,Repository forkedRepo)
    {
        ForkedRepo = forkedRepo;
        ForkedRepoId = forkedRepo.Id;
        SourceRepo = sourceRepo;
        SourceRepoId = sourceRepo.Id;
    }

    public static RepositoryFork Create(Repository sourceRepo,Repository forkedRepo)
    {
        return new RepositoryFork(sourceRepo, forkedRepo);
    }
}