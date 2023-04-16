namespace Trackable
{
    public interface ITracker<out T> where T : ITrackable
    {
        T Subject { get; }
    }
}
