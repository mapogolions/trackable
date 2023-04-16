using Microsoft.Extensions.Primitives;

namespace Trackable
{
    public interface ITrackable
    {
        IChangeToken GetToken();
    }
}
