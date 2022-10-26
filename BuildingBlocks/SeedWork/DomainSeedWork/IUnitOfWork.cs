// Copyright (c) .NET Foundation and Contributors. All Rights Reserved.
//
// Distributed under MIT license.
// See file LICENSE for detail or copy at https://opensource.org/licenses/MIT
// Modify by HongJieSun 2022

// Framework code of microservices and domain drive design pattern

using DomainSeedWork.Abstractions;

namespace DomainSeedWork
{
    /// <summary>
    /// 工作单元 Unit of Work是用来解决领域模型存储和变更工作，在ORM进行持久化的时候，比如Entity Framework的SaveChanges操作，其实就可以看做是Unit Of Work
    /// </summary>
    /// <remarks>
    /// 详情可以看:https://www.cnblogs.com/xishuai/p/3750154.html
    /// </remarks>
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        /// <summary>
        /// To call all domain events under a entity change.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken), bool saveChanges = true);
        Task<bool> SaveEntitiesAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default) where TEntity : Entity<string>, IAggregateRoot;
    }
}
