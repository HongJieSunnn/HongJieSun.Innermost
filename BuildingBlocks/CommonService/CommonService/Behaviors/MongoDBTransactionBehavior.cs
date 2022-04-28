using EventBusCommon.Abstractions;
using Innermost.MongoDBContext;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using MongoDBExtensions;
using TypeExtensions;

namespace CommonService.Behaviors
{
    /// <summary>
    /// MediatR behavior of MongoDB.
    /// </summary>
    /// <typeparam name="TMongoDBContext"></typeparam>
    /// <typeparam name="TIntegrationEventService"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TResponse"></typeparam>
    public class MongoDBTransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IClientSessionHandle _session;
        private readonly IIntegrationEventService _integrationEventService;
        private readonly ILogger<MongoDBTransactionBehavior<TRequest, TResponse>> _logger;
        public MongoDBTransactionBehavior(
            IClientSessionHandle session,
            IIntegrationEventService integrationEventService,
            ILogger<MongoDBTransactionBehavior<TRequest, TResponse>> logger
        )
        {
            _session = session;
            _integrationEventService = integrationEventService;
            _logger = logger;
        }
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var response = default(TResponse);
            var requestName = request.GetGenericTypeName();

            try
            {
                if (_session.IsInTransaction)
                    return await next();

                _session.StartTransaction();//TODO options

                _logger.LogInformation("----- Begin MongoDB transaction for {CommandName} ({@Command})",  requestName, request);

                response = await next();

                _logger.LogInformation("----- Commit MongoDB transaction {TransactionId} for {CommandName}", requestName);
                
                await _session.CommitTransactionAsync();

                await _integrationEventService.PublishEventsAsync(_session.GetSessionId());
            }
            catch (Exception ex)//TODO 
            {
                _logger.LogError(ex, "ERROR Handling transaction for {CommandName} ({@Command})", requestName, request);

                if (_session.IsInTransaction)
                    _session.AbortTransaction();

                throw;
            }

            return response;
        }
    }
}
