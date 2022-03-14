﻿using EventBusCommon;
using EventBusCommon.Abstractions;

namespace EventBusServiceBus
{
    public class EventBusAzureServiceBus : IAsyncEventBus, IAsyncDisposable
    {

        private readonly IServiceBusPersisterConnection _serviceBusPersisterConnection;
        private readonly ILogger<EventBusAzureServiceBus> _logger;
        private readonly IEventBusSubscriptionManager _subscriptionManager;
        private readonly ServiceBusProcessor _serviceBusProcessor;
        private readonly ServiceBusAdministrationClient _serviceBusAdministrationClient;
        private readonly ILifetimeScope _autofac;
        private readonly string AUTOFAC_SCOPE_NAME = "innermost_event_bus";
        /// <summary>
        /// 对于所有继承于IntegrationEvent的事件，都以该后缀结尾
        /// </summary>
        private const string INTEGRATION_EVENT_SUFFIX = "IntegrationEvent";
        private const string TOPIC_NAME = "innermost_event_bus_topic";
        private const string QUEUE_NAME = "innermost_event_bus_queue";
        private bool _disposed;

        public EventBusAzureServiceBus(IServiceBusPersisterConnection serviceBusPersisterConnection, ILogger<EventBusAzureServiceBus> logger,
            IEventBusSubscriptionManager subscriptionManager, string subscriptionName, ILifetimeScope autofac)
        {
            _serviceBusPersisterConnection = serviceBusPersisterConnection;
            _logger = logger ?? throw new ArgumentException(nameof(logger));
            _subscriptionManager = subscriptionManager ?? new InMemoryEventBusSubscriptionsManager();
            _serviceBusProcessor = _serviceBusPersisterConnection.CreateModel().CreateProcessor(TOPIC_NAME, subscriptionName,
                new ServiceBusProcessorOptions
                {
                    MaxConcurrentCalls = 10,
                    AutoCompleteMessages = false
                }
            );
            _serviceBusAdministrationClient = _serviceBusPersisterConnection.CreateAdministrationModel();
            _autofac = autofac;

            RemoveDefaultFilter().GetAwaiter().GetResult();
            RegisterProcessorMessageHandler();
        }

        public async Task Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name.Replace(INTEGRATION_EVENT_SUFFIX, "");
            var eventJsonStr = JsonConvert.SerializeObject(@event);
            var messageBody = new BinaryData(eventJsonStr);

            ServiceBusMessage message = new ServiceBusMessage
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = messageBody,
                Subject = eventName
            };

            await using (var sender = _serviceBusPersisterConnection.CreateModel().CreateSender(TOPIC_NAME))
            {
                await sender.SendMessageAsync(message);
            }
        }

        public async Task Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFFIX, "");
            var subscriptionName = typeof(TH).Name;

            var containsEvent = _subscriptionManager.HasSubscriptionForEvent<T>();
            if (!containsEvent)
            {
                try
                {
                    await _serviceBusAdministrationClient.CreateRuleAsync(TOPIC_NAME, subscriptionName, new CreateRuleOptions
                    {
                        Filter = new CorrelationRuleFilter { Subject = eventName },//添加筛选器，碰到 Subject 为 {eventName} 的消息，就会接受
                        Name = eventName
                    });
                }
                catch (ServiceBusException)
                {
                    _logger.LogWarning("The messaging entity {eventName} already exists.", eventName);
                }
            }

            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH).Name);

            _subscriptionManager.AddSubscription<T, TH>();
        }

        public async Task UnSubsribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler
        {
            var eventName = typeof(T).Name.Replace(INTEGRATION_EVENT_SUFFIX, "");
            var subscriptionName = typeof(TH).Name;

            try
            {
                await _serviceBusAdministrationClient.DeleteRuleAsync(TOPIC_NAME, subscriptionName, eventName);
            }
            catch (ServiceBusException)
            {
                _logger.LogWarning("The messaging entity {eventName} Could not be found.", eventName);
            }

            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _subscriptionManager.RemoveSubScription<T, TH>();
        }

        private async Task RemoveDefaultFilter()
        {
            var subscriptions = _serviceBusAdministrationClient.GetSubscriptionsAsync(TOPIC_NAME).GetAsyncEnumerator();
            try
            {
                while (await subscriptions.MoveNextAsync())
                {
                    await _serviceBusAdministrationClient.DeleteRuleAsync(TOPIC_NAME, subscriptions.Current.SubscriptionName, CreateRuleOptions.DefaultRuleName);
                }
            }
            catch (ServiceBusException)
            {
                _logger.LogWarning("The messaging entity {DefaultRuleName} Could not be found.", CreateRuleOptions.DefaultRuleName);
            }
        }
        /// <summary>
        /// 往processor的委托里传入处理消息和处理错误的函数，在发送一个message后，processor会invoke委托。
        /// </summary>
        private void RegisterProcessorMessageHandler()
        {
            _serviceBusProcessor.ProcessMessageAsync += ProcessMessage;
            _serviceBusProcessor.ProcessErrorAsync += ProcessError;
        }

        private async Task ProcessMessage(ProcessMessageEventArgs messageArgs)
        {
            //在subscriptionsManager中，eventName通过Type决定，没有去掉后缀
            var eventClassName = messageArgs.Message.Subject + INTEGRATION_EVENT_SUFFIX;
            var messageData = messageArgs.Message.Body.ToString();

            if (await ProcessEvent(eventClassName, messageData))
            {
                await messageArgs.CompleteMessageAsync(messageArgs.Message);
            }
        }

        private async Task<bool> ProcessEvent(string eventName, string messageData)
        {
            bool processed = false;

            if (_subscriptionManager.HasSubscriptionForEvent(eventName))
            {
                using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                {
                    var subscriptions = _subscriptionManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        //得到 handler 实例->获取正确的泛型类型->正确的类型通过 handler 实例调用 Handle 函数。
                        var handler = scope.ResolveOptional(subscription);
                        if (handler == null) continue;
                        var eventType = _subscriptionManager.GetEventTypeByName(eventName)!;//因为模板实现的handler类需要对应事件的类型，通过Publish时发送的Json存储的EventName获得EventType，Body反序列化得到对应事件 not null
                        var integrationEvent = JsonConvert.DeserializeObject(messageData, eventType);
                        var handlerConcreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);//将对应事件的类型填入模板组成完整的handler类型
                        await (Task)handlerConcreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                    }
                }
                processed = true;
            }
            return processed;
        }

        private Task ProcessError(ProcessErrorEventArgs errorArgs)
        {
            var ex = errorArgs.Exception;
            var context = errorArgs.ErrorSource;

            _logger.LogError(ex, "ERROR handling message: {ExceptionMessage} - ErrorSource: {@ExceptionContext}", ex.Message, context);

            return Task.CompletedTask;
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed) return;

            _subscriptionManager.Clear();
            try
            {
                await _serviceBusPersisterConnection.DisposeAsync();
                if (!_serviceBusProcessor.IsClosed)
                {
                    await _serviceBusProcessor.CloseAsync();
                }
                await _serviceBusProcessor.DisposeAsync();

                _disposed = true;
            }
            catch (IOException ex)
            {
                _logger.LogCritical(ex.ToString());
            }
        }
    }
}
