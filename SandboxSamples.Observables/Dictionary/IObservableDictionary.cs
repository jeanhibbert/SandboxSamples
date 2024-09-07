using System;
using System.Reactive;

namespace SandboxSamples.Observables;

public interface IObservableDictionary<TKey, TValue> : IObservable<DictionaryNotification<TKey, TValue>>, IDisposable
{
    IObservable<DictionaryNotification<TKey, TValue>> Get(TKey key);
    IObservable<Unit> IsInitialised();
}