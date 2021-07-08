namespace InvertedObserver
{
    public interface IObserver<T> where T : IObservable
    {
        T Subject { get; }
    }
}
