using DomainSeedWork;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MediatRExtensions
{
    public static class MediatRDispatchExtension
    {
        public static async Task DisPatchDomainEvents<TDbContext>(this IMediator mediatR, TDbContext context)
            where TDbContext : DbContext
        {
            var entities = context.ChangeTracker
                .Entries<Entity>()
                .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Any());

            var domainEvents = entities.SelectMany(e => e.Entity.DomainEvents).ToList();

            entities.ToList().ForEach(e => e.Entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                await mediatR.Publish(domainEvent);
            }
        }
    }
}
