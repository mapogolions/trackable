using System.Linq;
using Microsoft.Extensions.Primitives;

namespace Trackable
{
    public class GroupTrackable<T> : ITrackable
        where T : ITrackable
    {
        private readonly T[] _trackables;

        public GroupTrackable(params T[] trackables)
        {
            _trackables = trackables;
        }

        public IChangeToken GetToken() =>
            new CompositeChangeToken(_trackables.Select(x => x.GetToken()).ToArray());
    }

    public class GroupTrackable : GroupTrackable<ITrackable> { }
}
