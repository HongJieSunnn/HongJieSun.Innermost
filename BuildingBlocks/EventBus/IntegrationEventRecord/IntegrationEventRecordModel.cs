// Copyright (c) .NET Foundation and Contributors. All Rights Reserved.
//
// Distributed under MIT license.
// See file LICENSE for detail or copy at https://opensource.org/licenses/MIT
// Modify by HongJieSun 2022

// Framework code of microservices and domain drive design pattern

namespace IntegrationEventRecord
{
    public class IntegrationEventRecordModel
    {
        public Guid EventId { get; private set; }
        public string TransactionId { get; private set; }
        public string EventTypeName { get; private set; }
        public EventState State { get; set; }
        public DateTime CreateTime { get; private set; }
        public string EventContent { get; private set; }
        public int TimesSend { get; set; }//发送的次数，当该事件已在处理而又发送则TimesSend++
        [NotMapped]
        public string EventTypeShortName => EventTypeName.Split(".").Last();//去掉了名称空间的名
        [NotMapped]
        public IntegrationEvent? IntegrationEvent { get; private set; }

        private IntegrationEventRecordModel()
        {
        }

        public IntegrationEventRecordModel(IntegrationEvent @event, Guid transactionId)
        {
            EventId = @event.Id;
            TransactionId = transactionId.ToString();
            EventTypeName = @event.GetType().FullName??throw new NullReferenceException("The fullname of event is null.");
            State = EventState.NotPublished;
            CreateTime = @event.CreationDate;
            EventContent = JsonConvert.SerializeObject(@event);
            TimesSend = 0;
        }

        public IntegrationEventRecordModel DeserializeIntegrationEventFromEventContent(Type type)
        {
            IntegrationEvent = JsonConvert.DeserializeObject(EventContent, type) as IntegrationEvent;
            return this;
        }
    }
}
