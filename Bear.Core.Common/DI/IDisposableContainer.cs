using System;

namespace Bear.Core.Common.DI;

public interface IDisposableContainer : IDisposable
{
    void AddDisposableObj(IDisposable disposableObj);
}
