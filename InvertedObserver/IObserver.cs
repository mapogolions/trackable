namespace InvertedObserver
{
    public interface IObserver<T> where T : IObservable { }

    public interface IObserver : IObserver<IObservable> { }
}
