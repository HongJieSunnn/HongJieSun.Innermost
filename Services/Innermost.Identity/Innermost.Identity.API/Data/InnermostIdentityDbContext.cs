using IntegrationEventServiceSQL;
using IntegrationEventServiceSQL.EntityFrameworkConfigurations;

namespace Innermost.Identity.API.Data
{
    public class InnermostIdentityDbContext : IdentityDbContext<InnermostUser>
    {
        public DbSet<IntegrationEventSQLModel> IntegrationEvents { get; set; }
        public InnermostIdentityDbContext(DbContextOptions<InnermostIdentityDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<InnermostUser>().Property(user => user.Age).HasDefaultValue(1).IsRequired();//Age默认为1岁
            builder.Entity<InnermostUser>().Property(user => user.SelfDescription).HasDefaultValue("Be Yourself").IsRequired();//默认个人描述
            builder.Entity<InnermostUser>().Property(user => user.Birthday).HasMaxLength(10).HasDefaultValue("2000-01-01").IsRequired();//默认生日
            builder.Entity<InnermostUser>().Property(user => user.PhoneNumber).HasMaxLength(25);
            builder.Entity<InnermostUser>().Property(user => user.CreateTime).HasColumnType("DateTime").HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();//CreateTime默认为CURRENT_TIMESTAMP
            builder.Entity<InnermostUser>().Property(user => user.UpdateTime).HasColumnType("DateTime");
            builder.Entity<InnermostUser>().Property(user => user.DeleteTime).HasColumnType("DateTime");
            builder.Entity<InnermostUser>().Property(user => user.UserStatue).HasDefaultValue("NORMAL").IsRequired();
            builder.Entity<InnermostUser>().Property(user => user.UserAvatarUrl).HasMaxLength(220).HasDefaultValue("").IsRequired();
            builder.Entity<InnermostUser>().Property(user => user.UserBackgroundImageUrl).HasMaxLength(220).HasDefaultValue("https://innermost-user-img-1300228246.cos.ap-nanjing.myqcloud.com/backgrounds/default-bgimg.jpg").IsRequired();
            builder.ApplyConfiguration(new IntegrationEventSQLModelEntityTypeConfiguration());
        }
    }
}
