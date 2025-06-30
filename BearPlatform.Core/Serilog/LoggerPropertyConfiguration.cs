using BearPlatform.Core.ConfigOptions;
using Serilog.Context;
using SqlSugar;

namespace BearPlatform.Core.Serilog;

public class LoggerPropertyConfiguration : IDisposable
{
    private readonly Stack<IDisposable> _disposableStack = new();

    public static LoggerPropertyConfiguration Create => new();

    public void AddStock(IDisposable disposable)
    {
        _disposableStack.Push(disposable);
    }

    public IDisposable AddAopSqlProperty(ISqlSugarClient db, SerilogOptions serilogOptions)
    {
        AddStock(LogContext.PushProperty(LoggerProperty.RecordSqlLog, serilogOptions.RecordSqlEnabled));
        AddStock(LogContext.PushProperty(LoggerProperty.ToDb, serilogOptions.ToDb.Enabled));
        AddStock(LogContext.PushProperty(LoggerProperty.ToFile, serilogOptions.ToFile.Enabled));
        AddStock(LogContext.PushProperty(LoggerProperty.ToConsole, serilogOptions.ToConsole.Enabled));
        AddStock(LogContext.PushProperty(LoggerProperty.ToElasticsearch, serilogOptions.ToElasticsearch.Enabled));
        AddStock(LogContext.PushProperty(LoggerProperty.SugarActionType, db.SugarActionType));
        return this;
    }


    public void Dispose()
    {
        while (_disposableStack.Count > 0)
        {
            _disposableStack.Pop().Dispose();
        }
    }
}
