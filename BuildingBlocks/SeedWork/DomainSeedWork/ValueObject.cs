// Copyright (c) .NET Foundation and Contributors. All Rights Reserved.
//
// Distributed under MIT license.
// See file LICENSE for detail or copy at https://opensource.org/licenses/MIT
// Modify by HongJieSun 2022

// Framework code of microservices and domain drive design pattern

namespace DomainSeedWork
{
    /// <summary>
    /// Innermost中采用DDD的服务的 值对象基类
    /// </summary>
    public abstract class ValueObject
    {
        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
            {
                return false;
            }

            var other = (ValueObject)obj;

            return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());//比较继承了ValueObject的类型的所有成员函数是否相等（把所有成员函数通过GetEqualityComponents弄成枚举）
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
             .Select(x => x != null ? x.GetHashCode() : 0)
             .Aggregate((x, y) => x ^ y);
        }

        public ValueObject GetCopy()
        {
            return this.MemberwiseClone() as ValueObject;
        }
    }
}
