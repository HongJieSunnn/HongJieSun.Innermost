// Copyright (c) .NET Foundation and Contributors. All Rights Reserved.
//
// Distributed under MIT license.
// See file LICENSE for detail or copy at https://opensource.org/licenses/MIT
// Modify by HongJieSun 2022

// Framework code of microservices and domain drive design pattern
using EventBusCommon.Abstractions;

namespace EventBusCommon
{
    public interface IEventBusSubscriptionManager
    {
        public bool IsEmpty { get; }
        event EventHandler<string> OnEventRemoved;

        void AddSubscription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler;

        void RemoveSubScription<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler;

        bool HasSubscriptionForEvent<T>() where T : IntegrationEvent;
        bool HasSubscriptionForEvent(string eventName);
        Type? GetEventTypeByName(string eventName);
        void Clear();
        IEnumerable<Type> GetHandlersForEvent<T>() where T : IntegrationEvent;
        IEnumerable<Type> GetHandlersForEvent(string eventName);
        /// <summary>
        /// Get event's type name from T
        /// </summary>
        /// <typeparam name="T">event type</typeparam>
        /// <returns></returns>
        string GetEventKey<T>();
    }
}
