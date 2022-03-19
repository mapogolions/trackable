using System.Linq;
using Microsoft.Extensions.Primitives;

namespace InvertedObserver
{
    public class GroupObservable<T> where T : IObservable
    {
        private readonly T[] _observables;

        public GroupObservable(params T[] observables)
        {
            _observables = observables;
        }

        public IChangeToken GetReloadToken() =>
            new CompositeChangeToken(_observables.Select(x => x.GetReloadToken()).ToArray());
    }

    public class GroupObservable : GroupObservable<IObservable> { }
}
