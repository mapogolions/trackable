using System.Threading;
using Microsoft.Extensions.Primitives;

namespace InvertedObserver
{
    public abstract class Observable : IObservable
    {
        private CancellationTokenSource _cts = new();

        public virtual IChangeToken GetReloadToken() => new CancellationChangeToken(_cts.Token);

        protected virtual void RefreshToken()
        {
            var cts = _cts;
            _cts = new CancellationTokenSource();
            cts.Cancel();
        }
    }
}
