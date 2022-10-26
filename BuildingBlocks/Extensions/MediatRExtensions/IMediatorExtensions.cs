using DomainSeedWork.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace MediatR
{
    public static class IMediatorExtensions
    {
        public static async Task DisPatchDomainEvents<TDbContext>(this IMediator mediatR, TDbContext context)
            where TDbContext : DbContext
        {
            var entities = context.ChangeTracker
                .Entries<Entity<int>>()
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
