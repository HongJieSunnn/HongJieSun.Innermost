// Copyright (c) .NET Foundation and Contributors. All Rights Reserved.
//
// Distributed under MIT license.
// See file LICENSE for detail or copy at https://opensource.org/licenses/MIT
// Modify by HongJieSun 2022

// Framework code of microservices and domain drive design pattern

namespace DomainSeedWork.Abstractions
{
    public abstract class Entity<TId>
        where TId : IEquatable<TId>, IComparable<TId>
    {
        int? _requestedHashCode;
        TId? _Id;
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public virtual TId? Id
        {
            get { return _Id; }
            set { _Id = value; }
        }
        /// <summary>
        /// MediatR.Inotification 通知，也就是发布订阅中的事件
        /// </summary>
        [NotMapped]
        [BsonIgnore]
        private List<INotification>? _domainEvents;
        [NotMapped]
        [BsonIgnore]
        public virtual IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();
        public virtual void AddDomainEvent(INotification eventItem)
        {
            _domainEvents = _domainEvents ?? new List<INotification>();
            _domainEvents.Add(eventItem);
        }

        public virtual void RemoveDomainEvent(INotification eventItem)
        {
            _domainEvents?.Remove(eventItem);
        }

        public virtual void ClearDomainEvents()
        {
            _domainEvents?.Clear();
        }

        public virtual bool IsTransient()
        {
            return this.Id.Equals(default(TId)) && this.Id.CompareTo(default(TId)) == 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is Entity<TId>))
                return false;

            if (Object.ReferenceEquals(this, obj))//如果指向同一实例，相等。
                return true;

            if (this.GetType() != obj.GetType())
                return false;

            Entity<TId> entity = (Entity<TId>)obj;
            //实体如果是临时的，那么不可能存在和它一样的另一个实体
            //if id is null and will also return false
            if (entity.IsTransient() || this.IsTransient())
                return false;
            else
                return entity.Id.Equals(this.Id);//如果不是一个实例，但是Id相等，由于实体具有唯一的Id，所以也相等。
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                {
                    _requestedHashCode = this.Id.GetHashCode() ^ 31;
                }
                return _requestedHashCode.Value;
            }
            return base.GetHashCode();
        }

        public static bool operator ==(Entity<TId> left, Entity<TId> right)
        {
            if (Object.Equals(left, null))
                return (Object.Equals(right, null)) ? true : false;
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity<TId> left, Entity<TId> right)
        {
            return !(left == right);
        }
    }
}
