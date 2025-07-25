using SqlSugar;

namespace BearPlatform.Repository.UnitOfWork;

public interface IUnitOfWork
{
    SqlSugarScope GetDbClient();
    void BeginTran();
    void CommitTran();
    void RollbackTran();
}
