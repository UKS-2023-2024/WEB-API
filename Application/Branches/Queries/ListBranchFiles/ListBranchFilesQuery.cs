using Application.Shared;
using Domain.Branches;

namespace Application.Branches.Queries.ListBranchFiles;

public record ListBranchFilesQuery(Guid BranchId, string Folder) : IQuery<List<ContributionFile>>;
