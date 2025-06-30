using System;

namespace BearPlatform.Common.DI;

public interface IDisposableContainer : IDisposable
{
    void AddDisposableObj(IDisposable disposableObj);
}
