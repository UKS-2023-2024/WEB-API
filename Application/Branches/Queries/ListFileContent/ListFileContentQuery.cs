using Application.Shared;
using Domain.Branches;

namespace Application.Branches.Queries.ListFileContent;

public record ListFileContentQuery(Guid BranchId, string Path) : IQuery<FileContent?>;
