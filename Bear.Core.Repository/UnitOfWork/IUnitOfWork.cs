using SqlSugar;

namespace Bear.Core.Repository.UnitOfWork;

public interface IUnitOfWork
{
    SqlSugarScope GetDbClient();
    void BeginTran();
    void CommitTran();
    void RollbackTran();
}
