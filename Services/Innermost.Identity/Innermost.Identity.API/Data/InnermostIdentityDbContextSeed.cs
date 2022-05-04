using EventBusCommon.Abstractions;
using Innermost.Identity.API.IntegrationEvents;

namespace Innermost.Identity.API.Data
{
    public class InnermostIdentityDbContextSeed
    {
        private readonly IPasswordHasher<InnermostUser> _passwordHasher = new PasswordHasher<InnermostUser>();
        public async Task SeedAsync(InnermostIdentityDbContext context, UserManager<InnermostUser> userManager,IIntegrationEventService integrationEventService,IUserStatueService userStatueService, IConfiguration configuration)
        {
            if (context.Users.Any())
                return;
            var userToAdd = DefaultUsers();

            await context.Users.AddRangeAsync(userToAdd);
            await context.SaveChangesAsync();

            var adminUser = userToAdd[0];
            var hongJieSunUser = userToAdd[1];
            var testerUser = userToAdd[2];

            await ConfigureSeedUser(userManager, integrationEventService, adminUser);
            await ConfigureSeedUser(userManager, integrationEventService, hongJieSunUser);
            await ConfigureSeedUser(userManager, integrationEventService, testerUser);

            await ConfigureSeedUserStatue(userStatueService, adminUser.Id);//Admin always offlined.
            await ConfigureSeedUserStatue(userStatueService, hongJieSunUser.Id);
            await ConfigureSeedUserStatue(userStatueService, testerUser.Id);
        }

        private async Task ConfigureSeedUser(UserManager<InnermostUser> userManager,IIntegrationEventService integrationEventService,InnermostUser user)
        {
            await userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, "User"));
            await userManager.AddClaimAsync(user, new Claim("nickname", user.NickName));
            await userManager.AddClaimAsync(user, new Claim("user_statue", user.UserStatue));
            await userManager.AddClaimAsync(user, new Claim("avatarimg", user.UserAvatarUrl));
            await userManager.AddClaimAsync(user, new Claim("backgroundimg", user.UserBackgroundImageUrl));
            var userToken = await userManager.GenerateEmailConfirmationTokenAsync(user);
            await userManager.ConfirmEmailAsync(user, userToken);

            var integrationEvent = new UserRegisteredIntegrationEvent(user.Id);
            await integrationEventService.SaveEventAsync(integrationEvent);
            await integrationEventService.PublishEventsAsync(new[] {integrationEvent.Id});
        }

        private async Task ConfigureSeedUserStatue(IUserStatueService userStatueService,string userId,bool onlined=false)
        {
            await userStatueService.SetUserOnlineStatueAsync(userId, onlined);
            await userStatueService.SetUserStatueAsync(userId, "NORMAL");
        }

        public List<InnermostUser> DefaultUsers()
        {
            var admin = new InnermostUser()
            {
                Id = "13B8D30F-CFF8-20AB-8D40-1A64ADA8D067",
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "Admin@innermost.com",
                NormalizedEmail = "ADMIN@INNERMOST.COM",
                Gender = "MALE",
                NickName = "Admin",
                CreateTime = DateTime.Now,
            };
            var hongjiesunUser = new InnermostUser()
            {
                Id = "555f5f9b-ebf8-4d75-8cd9-fb34f95f921d",
                UserName = "HongJieSun",
                NormalizedUserName = "HONGJIESUN",
                Email = "457406475@qq.com",
                NormalizedEmail = "457406475@QQ.COM",
                Age = 20,
                Gender = "MALE",
                NickName = "Deficienthonnn",
                School = "Nanjing Tech University",
                Province = "福建省",
                City = "福州市",
                SelfDescription = "I am HongJieSun",
                Birthday = "2000-08-26",
                CreateTime = DateTime.Now,
                PhoneNumber = "18506013757",
                UserAvatarUrl = "https://innermost-user-img-1300228246.cos.ap-nanjing.myqcloud.com/avatars/13ED0CD35270F8C5850930EB2975B12F.jpg"
            };
            var test = new InnermostUser()
            {
                UserName = "Tester",
                NormalizedUserName = "TESTER",
                Email = "Test@Innermost.com",
                NormalizedEmail = "Test@Innermost.com".ToUpper(),
                Age = 16,
                Gender = "FEMALE",
                NickName = "TestLover",
                School = "No.1 Middle School of Lianjiang",
                Province = "福建省",
                City = "福州市",
                SelfDescription = "I am Tester for Innermsot",
                Birthday = "2004-08-27",
                CreateTime = DateTime.Now,
                PhoneNumber = "12345678901",
            };

            admin.PasswordHash = _passwordHasher.HashPassword(hongjiesunUser, "Admin@Innermost");
            hongjiesunUser.PasswordHash = _passwordHasher.HashPassword(hongjiesunUser, "hong456..");
            test.PasswordHash = _passwordHasher.HashPassword(test, "testPwd");

            return new List<InnermostUser>()
            {
                admin,
                hongjiesunUser,
                test
            };
        }
    }
}
