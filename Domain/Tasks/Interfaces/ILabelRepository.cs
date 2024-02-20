using Domain.Shared.Interfaces;

namespace Domain.Tasks.Interfaces;

public interface ILabelRepository: IBaseRepository<Label>
{
    Task<List<Label>> FindAllByIds(Guid repoId, List<Guid> labelsIds);
    Task<Label> FindByRepositoryIdAndTitle(Guid repositoryId, string title);
}