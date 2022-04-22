namespace Innermost.Identity.API.Data
{
    public class InnermostIdentityDbContext : IdentityDbContext<InnermostUser>
    {
        public InnermostIdentityDbContext(DbContextOptions<InnermostIdentityDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<InnermostUser>().Property(user => user.Age).HasDefaultValue(1).IsRequired();//Age默认为1岁
            builder.Entity<InnermostUser>().Property(user => user.SelfDescription).HasDefaultValue("Be Yourself").IsRequired();//默认个人描述
            builder.Entity<InnermostUser>().Property(user => user.Birthday).HasDefaultValue("2000-01-01").IsRequired();//默认生日
            builder.Entity<InnermostUser>().Property(user => user.PhoneNumber).HasMaxLength(25);
            builder.Entity<InnermostUser>().Property(user => user.CreateTime).HasColumnType("DateTime").HasDefaultValueSql("CURRENT_TIMESTAMP").IsRequired();//CreateTime默认为CURRENT_TIMESTAMP
            builder.Entity<InnermostUser>().Property(user => user.UpdateTime).HasColumnType("DateTime");
            builder.Entity<InnermostUser>().Property(user => user.DeleteTime).HasColumnType("DateTime");
            builder.Entity<InnermostUser>().Property(user => user.UserStatue).HasDefaultValue("NORMAL").IsRequired();
            builder.Entity<InnermostUser>().Property(user => user.UserAvatarUrl).HasDefaultValue("").IsRequired();//TODO default avatar url
            builder.Entity<InnermostUser>().Property(user => user.UserBackgroundImageUrl).HasDefaultValue("").IsRequired();//TODO default backgroundimg url
        }
    }
}
