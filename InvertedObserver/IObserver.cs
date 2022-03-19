namespace InvertedObserver
{
    public interface IObserver<out T> where T : IObservable
    {
        T Subject { get; }
    }
}
