﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;


namespace SandboxSamples.Observables.Tests.Samples;

/// <summary>
/// This demonstrates receiving a stream of updates from a server which are made up of an initial full snapshot, then deltas.
/// </summary>
public class Deltas
{
    private Subject<Either<FullUpdate, DeltaUpdate>> _serverUpdateStream = new Subject<Either<FullUpdate, DeltaUpdate>>();
    private IObservable<DictionaryModification<string, Update>> _dictionaryModificationStream;
    private IObservableDictionary<string, Update> _observableDictionary;

    public Deltas()
    {
        _serverUpdateStream = new Subject<Either<FullUpdate, DeltaUpdate>>();

        _dictionaryModificationStream = _serverUpdateStream
            .Select(either =>
            {
                if (either.IsLeft)
                {
                    return DictionaryModification.Replace(either.Left.Values["key"], (Update)either.Left);
                }
                else
                {
                    return DictionaryModification.Upset(either.Right.Values["key"], (Update)either.Right);
                }
            });

        _observableDictionary = _dictionaryModificationStream.ToObservableDictionary((key, existing, update) =>
        {
            var fullUpdate = new FullUpdate(key)
            {
                Values = new Dictionary<string, string>(existing.Values)
            };

            foreach (var kvp in update)
            {
                fullUpdate.Values[kvp.Key] = kvp.Value;
            }

            return fullUpdate;
        }, CurrentThreadScheduler.Instance);
    }

    [Fact]
    public void Initial_snapshot_and_subsequent_deltas_yield_full_snapshot_each_time()
    {
        var key = "EURUSD";
        // arrange
        var observations = new List<Update>();
        _observableDictionary.Get(key)
            .Only(DictionaryNotificationType.Values) // ignore meta notifications
            .Select(dn => dn.Value) // select out the new value in the dictionary
            .Subscribe(observations.Add);

        // act
        _serverUpdateStream.OnNext(new Either<FullUpdate, DeltaUpdate>(new FullUpdate(key) { { "bid", "1.234"}, { "ask", "1.334"}, { "valueDate", "2014-07-16"} }));
        _serverUpdateStream.OnNext(new Either<FullUpdate, DeltaUpdate>(new DeltaUpdate(key) { { "bid", "1.233" }}));
        _serverUpdateStream.OnNext(new Either<FullUpdate, DeltaUpdate>(new DeltaUpdate(key) { { "ask", "1.333" }}));
        _serverUpdateStream.OnNext(new Either<FullUpdate, DeltaUpdate>(new DeltaUpdate(key) { { "bid", "1.231" }, { "ask", "1.331" }}));

        // assert

        observations.ForEach(update => Assert.Equal(4, update.Values.Count));
        var first = observations.First();
        var last = observations.Last();

        Assert.Equal("1.234", first.Values["bid"]);
        Assert.Equal("1.334", first.Values["ask"]);

        Assert.Equal("1.231", last.Values["bid"]);
        Assert.Equal("1.331", last.Values["ask"]);
    }

    [Fact]
    public void Initial_snapshot_and_subsequent_deltas_yield_full_snapshot_then_delta_each_time()
    {
        var key = "EURUSD";
        // arrange
        var observations = new List<Update>();
        _observableDictionary.Get(key)
            .Only(DictionaryNotificationType.Values) // ignore meta notifications
            .Select(dn =>
                dn.Type == DictionaryNotificationType.Inserted
                ? dn.Value
                : dn.UpdatingValue
            ) // select out the new value in the dictionary
            .Subscribe(observations.Add);

        // act
        _serverUpdateStream.OnNext(new Either<FullUpdate, DeltaUpdate>(new FullUpdate(key) { { "bid", "1.234" }, { "ask", "1.334" }, { "valueDate", "2014-07-16" } }));
        _serverUpdateStream.OnNext(new Either<FullUpdate, DeltaUpdate>(new DeltaUpdate(key) { { "bid", "1.233" } }));
        _serverUpdateStream.OnNext(new Either<FullUpdate, DeltaUpdate>(new DeltaUpdate(key) { { "ask", "1.333" } }));
        _serverUpdateStream.OnNext(new Either<FullUpdate, DeltaUpdate>(new DeltaUpdate(key) { { "bid", "1.231" }, { "ask", "1.331" }}));
        // assert

        Assert.Equal(4, observations[0].Values.Count);
        Assert.Equal(2, observations[1].Values.Count);
        Assert.Equal(2, observations[2].Values.Count);
        Assert.Equal(3, observations[3].Values.Count);
    }

    [Fact]
    public void Subscription_to_key_always_yields_full_snapshot_first()
    {
        var key = "EURUSD";
        // arrange
        var firstObservations = new List<Update>();
        _observableDictionary.Get(key)
            .Only(DictionaryNotificationType.Values) // ignore meta notifications
            .Select(dn =>
                dn.Type == DictionaryNotificationType.Inserted
                ? dn.Value
                : dn.UpdatingValue
            ) // select out the new value in the dictionary
            .Subscribe(firstObservations.Add);

        // act
        _serverUpdateStream.OnNext(new Either<FullUpdate, DeltaUpdate>(new FullUpdate(key) { { "bid", "1.234" }, { "ask", "1.334" }, { "valueDate", "2014-07-16" } }));
        _serverUpdateStream.OnNext(new Either<FullUpdate, DeltaUpdate>(new DeltaUpdate(key) { { "bid", "1.233" } }));
        _serverUpdateStream.OnNext(new Either<FullUpdate, DeltaUpdate>(new DeltaUpdate(key) { { "ask", "1.333" } }));
        
        var secondObservations = new List<Update>();
        _observableDictionary.Get(key)
            .Only(DictionaryNotificationType.Values) // ignore meta notifications
            .Select(dn =>
                dn.Type == DictionaryNotificationType.Updated
                ? dn.UpdatingValue
                : dn.Value
            ) // select out the new value in the dictionary
            .Subscribe(secondObservations.Add);

        
        _serverUpdateStream.OnNext(new Either<FullUpdate, DeltaUpdate>(new DeltaUpdate(key) { { "bid", "1.231" }, { "ask", "1.331" } }));
        // assert

        Assert.Equal(4, firstObservations.Count);
        Assert.Equal(2, secondObservations.Count);

        Assert.Equal(4, secondObservations[0].Values.Count);

    }
}