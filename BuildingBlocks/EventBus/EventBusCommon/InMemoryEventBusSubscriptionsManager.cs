// Copyright (c) .NET Foundation and Contributors. All Rights Reserved.
//
// Distributed under MIT license.
// See file LICENSE for detail or copy at https://opensource.org/licenses/MIT
// Modify by HongJieSun 2022

// Framework code of microservices and domain drive design pattern
using EventBusCommon.Abstractions;

namespace EventBusCommon
{
    public class InMemoryEventBusSubscriptionsManager : IEventBusSubscriptionManager
    {
        /// <summary>
        /// key:eventName value:subscriptions who subscribe this event
        /// </summary>
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        /// <summary>
        /// 外部传入函数，当EventRemoved，会invoke该委托，然后执行外部传入的函数从而执行Event被Removed后需要执行的某些操作。
        /// 例如可能需要重新添加事件等操作#2021.2.20
        /// 可看RabbitMQ连接中，它就传递了三个函数使连接关闭后继续重试连接
        /// </summary>
        public event EventHandler<string>? OnEventRemoved;
        public bool IsEmpty => !_handlers.Any();
        public void Clear() => _handlers.Clear();

        public InMemoryEventBusSubscriptionsManager()
        {
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
        }

        public void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler
        {
            var eventName = GetEventKey<T>();
            var eventType = typeof(T);
            var eventHandlerType = typeof(TH);

            DoAddSubscription(eventHandlerType, eventName);

            if (!_eventTypes.Contains(eventType))
            {
                _eventTypes.Add(eventType);
            }
        }

        /// <summary>
        /// Do AddSubscription function
        /// </summary>
        /// <param name="handlerType">subscription type</param>
        /// <param name="eventName">eventName</param>
        /// <param name="isDynamic"></param>
        private void DoAddSubscription(Type handlerType, string eventName)
        {
            if (!HasSubscriptionForEvent(eventName))
            {
                _handlers.Add(eventName, new List<Type>());
            }

            if (_handlers[eventName].Any(sub => sub == handlerType))
            {
                throw new ArgumentException(
                    $"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            _handlers[eventName].Add(handlerType);
        }

        public void RemoveSubScription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler
        {
            var eventName = GetEventKey<T>();
            var subscription = FindSubscriptionToRemove<T, TH>();

            DoRemoveSubScription(eventName, subscription);
        }

        /// <summary>
        /// Do RemoveSubScription function.
        /// </summary>
        /// <param name="eventName">eventName</param>
        /// <param name="subscription">subscription of one event want to be removed</param>
        private void DoRemoveSubScription(string eventName, Type? subscription)
        {
            if (subscription != null)
            {
                _handlers[eventName].Remove(subscription);
                if (!_handlers[eventName].Any())
                {
                    _handlers.Remove(eventName);
                    var eventType = _eventTypes.FirstOrDefault(e => e.Name == eventName);
                    if (eventType != null)
                    {
                        _eventTypes.Remove(eventType);
                    }
                    RaiseOnEventRemoved(eventName);
                }
            }
        }

        /// <summary>
        /// Find Subscription to remove
        /// </summary>
        /// <typeparam name="T">eventType</typeparam>
        /// <typeparam name="TH">handlerType</typeparam>
        /// <returns></returns>
        private Type? FindSubscriptionToRemove<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler
        {
            var eventName = typeof(T).Name;
            var handlerType = typeof(TH);

            return DoFindSubscriptionToRemove(eventName, handlerType);
        }

        /// <summary>
        /// Do FindSubscription to remove
        /// </summary>
        /// <param name="eventName">eventName from eventType</param>
        /// <param name="handlerType">handlerType from template TH</param>
        /// <returns></returns>
        private Type? DoFindSubscriptionToRemove(string eventName, Type handlerType)
        {
            if (!HasSubscriptionForEvent(eventName))
            {
                return null;
            }

            return _handlers[eventName].FirstOrDefault(s => s == handlerType);
        }

        public bool HasSubscriptionForEvent<T>() where T : IntegrationEvent
        {
            var eventName = typeof(T).Name;
            return HasSubscriptionForEvent(eventName);
        }

        public bool HasSubscriptionForEvent(string eventName)
        {
            return _handlers.ContainsKey(eventName);
        }

        public IEnumerable<Type> GetHandlersForEvent<T>() where T : IntegrationEvent
        {
            var eventName = GetEventKey<T>();
            return GetHandlersForEvent(eventName);
        }

        public IEnumerable<Type> GetHandlersForEvent(string eventName) => _handlers[eventName];

        public Type? GetEventTypeByName(string eventName) => _eventTypes.FirstOrDefault(t => t.Name == eventName);

        public string GetEventKey<T>()
        {
            return typeof(T).Name;
        }

        /// <summary>
        /// while eventName's appropriate event is being removed.Raise the delegate OnEventRemoved.
        /// </summary>
        /// <param name="eventName"></param>
        private void RaiseOnEventRemoved(string eventName)
        {
            var handler = OnEventRemoved;
            handler?.Invoke(this, eventName);
        }
    }
}
