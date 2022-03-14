// Copyright (c) .NET Foundation and Contributors. All Rights Reserved.
//
// Distributed under MIT license.
// See file LICENSE for detail or copy at https://opensource.org/licenses/MIT
// Modify by HongJieSun 2022

// Framework code of microservices and domain drive design pattern

using Serilog.Context;

namespace Innermost.LogLife.API.Application.Behaviors
{
    /// <summary>
    /// Behavior of MediatR for Transaction.
    /// To public all domain events under a transaction.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly LifeRecordDbContext _dbContext;
        private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
        private readonly ILogLifeIntegrationEventService _integrationEventService;
        public TransactionBehavior(LifeRecordDbContext context, ILogger<TransactionBehavior<TRequest, TResponse>> logger, ILogLifeIntegrationEventService integrationEventService)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(LifeRecordDbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(Microsoft.Extensions.Logging.ILogger));
            _integrationEventService = integrationEventService ?? throw new ArgumentNullException(nameof(ILogLifeIntegrationEventService));
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var requestName = request.GetGenericTypeName();

            try
            {
                if (_dbContext.HasActiveTrasaction)
                {
                    return await next();
                }

                var stragegy = _dbContext.Database.CreateExecutionStrategy();

                await stragegy.ExecuteAsync(async () =>
                {
                    Guid transactionId;
                    //当通过dispatchdomainevents方法来publish一系列的领域事件时，每个事件都会触发该behavior
                    //但是由于同时只能存在一个事务，所以虽然每个都BeginTransaction但实际上有些是获得的_currentTransaction，所以一个transactionId可能有多个事件需要处理
                    using (var transaction = await _dbContext.BeginTransactionAsync())
                    using (LogContext.PushProperty("TransactionContext", transaction.TransactionId))
                    {
                        _logger.LogInformation("----- Begin transaction {TransactionId} for {CommandName} ({@Command})", transaction.TransactionId, requestName, request);

                        response = await next.Invoke();
                        //when arrive here.The domainEvents called by this command have been handled.

                        _logger.LogInformation("----- Commit transaction {TransactionId} for {CommandName}", transaction.TransactionId, requestName);

                        await _dbContext.CommitTransactionAsync(transaction);

                        transactionId = transaction.TransactionId;
                    }

                    await _integrationEventService.PublishEventsAsync(transactionId);
                });

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error Handling transaction for {requestName} ({request})");
                throw;
            }
        }
    }
}
