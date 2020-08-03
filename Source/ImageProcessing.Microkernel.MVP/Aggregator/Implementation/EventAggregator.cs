using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using ImageProcessing.Microkernel.MVP.Aggregator.Interface;
using ImageProcessing.Microkernel.MVP.Aggregator.Subscriber;

namespace ImageProcessing.Microkernel.MVP.Aggregator.Implementation
{
    /// <inheritdoc cref="IEventAggregator"/>
    internal sealed class EventAggregator : IEventAggregator
    {
        private readonly object _syncRoot = new object();

        private readonly Dictionary<Type, HashSet<(object, object)>> _eventSubscribers
            = new Dictionary<Type, HashSet<(object, object)>>();

        /// <inheritdoc cref="IEventAggregator.PublishFrom{TEventArgs}(object, TEventArgs)"
        public void PublishFrom<TEventArgs>(object publisher, TEventArgs args)
        {
            var subsriberType = typeof(ISubscriber<>).MakeGenericType(typeof(TEventArgs));

            lock (_syncRoot)
            {
                var pairs = GetSubscribers(subsriberType).Where((pair) => pair.Publisher == publisher);

                Publish(pairs, args);
            }
        }


        /// <inheritdoc cref="IEventAggregator.PublishFromAll{TEventArgs}(TEventArgs)"
        public void PublishFromAll<TEventArgs>(TEventArgs args)
        {
            var subsriberType = typeof(ISubscriber<>).MakeGenericType(typeof(TEventArgs));

            lock (_syncRoot)
            {
                Publish(GetSubscribers(subsriberType), args);
            }
        }

        /// <inheritdoc cref="IEventAggregator.Subscribe(object, object)"/>
        public void Subscribe(object subscriber, object publisher)
        {
            var subscriberType = subscriber.GetType();

            var subsriberTypes = subscriberType.GetInterfaces()
                .Where(i => i.IsGenericType &&
                       i.GetGenericTypeDefinition() == typeof(ISubscriber<>));

            lock (_syncRoot)
            {
                foreach (var subsriberType in subsriberTypes)
                {
                    var subscribers = GetSubscribers(subsriberType);

                    if (!subscribers.Any(pair => pair.Subscriber == subscriber))
                    {
                        subscribers.Add((subscriber, publisher));
                    }             
                }
            }
        }

        /// <inheritdoc cref="IEventAggregator.Unsubscribe(Type, publisher))"/>
        public void Unsubscribe(Type subscriber, object publisher)
        {
            var subsriberTypes = subscriber.GetInterfaces()
                .Where(i => i.IsGenericType &&
                       i.GetGenericTypeDefinition() == typeof(ISubscriber<>));

            lock (_syncRoot)
            {
                foreach (var subsriberType in subsriberTypes)
                {
                    var subscribers = GetSubscribers(subsriberType);
                        subscribers.RemoveWhere(
                            pair => pair.Publisher == publisher
                        );
                }
            }
        }

        private void Publish<TEventArgs>(
            IEnumerable<(object Subscriber, object Publisher)> pairs,
            TEventArgs args)
        {
            foreach (var pair in pairs)
            {
                var subscriber = pair.Subscriber as ISubscriber<TEventArgs>;

                if (subscriber != null)
                {
                    InvokeSubscriberEvent(args, subscriber);
                }
            }
        }

        private void InvokeSubscriberEvent<TEventType>(
            TEventType publisher,
            ISubscriber<TEventType> subscriber)
        {
            var syncContext = SynchronizationContext.Current;

            if (syncContext is null)
            {
                syncContext = new SynchronizationContext();
            }

            syncContext.Post(s => subscriber.OnEventHandler(publisher), null);
        }

        private HashSet<(object Subscriber, object Publisher)> GetSubscribers(Type subsriberType)
        {
            lock (_syncRoot)
            {
                var isFound = _eventSubscribers
                    .TryGetValue(
                        subsriberType, out var subsribers
                     );

                if (!isFound)
                {
                    subsribers = new HashSet<(object, object)>();

                    _eventSubscribers.Add(subsriberType, subsribers);
                }

                return subsribers;
            }
        }
    }
}
