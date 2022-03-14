// Copyright (c) .NET Foundation and Contributors. All Rights Reserved.
//
// Distributed under MIT license.
// See file LICENSE for detail or copy at https://opensource.org/licenses/MIT
// Modify by HongJieSun 2022

// Framework code of microservices and domain drive design pattern

namespace DomainSeedWork
{
    /// <summary>
    /// 枚举，可以派生各种状态。
    /// </summary>
    public abstract class Enumeration : IComparable
    {
        public int Id { get; private init; }
        public string Name { get; private init; }
        public Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public override bool Equals(object obj)
        {
            var objEnumeration = obj as Enumeration;
            if (objEnumeration == null)
                return false;
            return this.GetType().Equals(obj.GetType()) && this.Id.Equals(objEnumeration.Id);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
        public int CompareTo(object obj)
        {
            return Id.CompareTo(((Enumeration)obj).Id);
        }
    }
}
