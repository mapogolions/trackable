using System.Threading;
using Microsoft.Extensions.Primitives;

namespace Trackable
{
    public abstract class Trackable : ITrackable
    {
        private CancellationTokenSource _cts = new();

        public virtual IChangeToken GetToken() => new CancellationChangeToken(_cts.Token);

        protected virtual void RefreshToken()
        {
            var cts = _cts;
            _cts = new CancellationTokenSource();
            cts.Cancel();
        }
    }
}
