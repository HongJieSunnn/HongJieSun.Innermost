// Copyright (c) .NET Foundation and Contributors. All Rights Reserved.
//
// Distributed under MIT license.
// See file LICENSE for detail or copy at https://opensource.org/licenses/MIT

// Framework code of microservices and domain drive design pattern

namespace IntegrationEventRecord.Utilities
{
    /// <summary>
    /// 这个类存在的作用是，通过传入的 DbContext 实例开启一个事务来支持IntegrationEventRecord的事务性。
    /// </summary>
    /// <remarks>
    /// 往往是用于使用非DDD的微服务，因为DDD的微服务会在传送领域事件时自动开启事务，但是普通的服务并不会，所以要通过这个类来开启事务。
    /// </remarks>
    public class ResilientTransaction
    {
        private readonly DbContext _dbContext;

        private ResilientTransaction(DbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        ResilientTransaction New(DbContext dbContext) => new ResilientTransaction(dbContext);

        public async Task ExecuteAsync(Func<Task> action)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    await action();
                    transaction.Commit();
                }
            });
        }
    }
}
