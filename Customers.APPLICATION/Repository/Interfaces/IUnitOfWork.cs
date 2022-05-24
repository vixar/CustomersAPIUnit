namespace Application.Repository.Interfaces;

public interface IUnitOfWork
{
    Task<int> Commit(CancellationToken cancellationToken);
    Task Rollback();

}