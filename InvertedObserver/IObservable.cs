using System;
using Microsoft.Extensions.Primitives;

namespace InvertedObserver
{
    public interface IObservable
    {
        IChangeToken GetReloadToken();
    }
}
