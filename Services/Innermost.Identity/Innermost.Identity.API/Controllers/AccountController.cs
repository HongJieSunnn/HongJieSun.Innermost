using EventBusCommon.Abstractions;
using Innermost.Identity.API.IntegrationEvents;

namespace Innermost.Identity.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILoginService<InnermostUser> _loginService;
        private readonly IIdentityServerInteractionService _identityServerInteraction;
        private readonly IUserStatueService _userStatueService;
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<InnermostUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IIntegrationEventService _integrationEventService;

        public AccountController(
            ILoginService<InnermostUser> loginService,
            IIdentityServerInteractionService identityServerInteraction,
            IUserStatueService userStatueService,
            ILogger<AccountController> logger,
            UserManager<InnermostUser> userManager,
            IConfiguration configuration,
            IIntegrationEventService integrationEventService
        )
        {
            _loginService = loginService;
            _identityServerInteraction = identityServerInteraction;
            _userStatueService = userStatueService;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
            _integrationEventService = integrationEventService;
        }

        [Route("Register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterModel model, string? returnUrl = null)
        {
            if (model.Password != model.ConfirmPassword)
                return BadRequest("Password does not equal to ComfirmPassword.");

            var age = DateTime.Now.Year - model.Birthday.Year;
            var newUser = new InnermostUser(
                model.UserName, model.Email,
                age, model.Gender, model.NickName,
                model.School, model.Province, model.City,
                model.SelfDescription,
                model.Birthday.ToString("yyyy-MM-dd"),
                model.UserAvatarUrl, model.UserBackgroundImageUrl,
                DateTime.Now
            );

            var createNewUserRes = await _userManager.CreateAsync(newUser, model.Password);
            if (createNewUserRes.Errors.Count() > 0)
            {
                return BadRequest(await BuildErrorModelStateJsonStr(model, createNewUserRes.Errors.Select(error => error.Description)));
            }

            var createdUser = await _userManager.FindByNameAsync(model.UserName);

            await AddUserStatuesToRedisAsync(createdUser.Id);
            await AddClaimsToUserAsync(createdUser);
            await SendConfirmEmailAsync(createdUser);
            await PublishUserRegisteredIntegrationEventToMeet(createdUser.Id);

            if (returnUrl != null && HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return Redirect(returnUrl);
            }

            return Ok();
        }

        private async Task AddClaimsToUserAsync(InnermostUser createdUser)
        {

            await _userManager.AddClaimsAsync(createdUser, new[]
            {
                new Claim(ClaimTypes.Role,"User"),
                new Claim("nickname",createdUser.NickName),
                new Claim("gender",createdUser.Gender),
                new Claim("user_statue", createdUser.UserStatue),
                new Claim("avatarimg", createdUser.UserAvatarUrl),
                new Claim("backgroundimg", createdUser.UserBackgroundImageUrl),
            });
        }

        private async Task AddUserStatuesToRedisAsync(string userId)
        {
            await _userStatueService.SetUserOnlineStatueAsync(userId, false);
            await _userStatueService.SetUserStatueAsync(userId, "NORMAL");
        }

        private async Task SendConfirmEmailAsync(InnermostUser user)
        {
            var confirmToken =await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var expiredTime = DateTime.Now.AddMinutes(10).ToString("yyyy-MM-ddTHH:mm:ss");

            var confirmTokenEscaped = Uri.EscapeDataString(confirmToken);
            var confirmUri=new Uri($"https://localhost:5106/account/email-confirm?userId={user.Id}&confirmToken={confirmTokenEscaped}&expiredTime={expiredTime}");

            var body = 
                $@"
                    Hi,<b><font size={"5"}>{user.NickName}</font></b> @{user.UserName}<br/><br/>
                    感谢您注册 <b>Innermost</b> <br/> 
                    请点击该链接进行账号邮箱验证:<a href={confirmUri}>验证地址</a><br/>
                    请在 {expiredTime.Replace('T',' ')} 前完成验证
                ";
            var sendEmailIntegrationEvent = new SendMailIntegrationEvent(user.Email, "Innermost账号注册邮箱验证", body, true);
            await _integrationEventService.SaveEventAsync(sendEmailIntegrationEvent);
            await _integrationEventService.PublishEventsAsync(new[] { sendEmailIntegrationEvent.Id });
        }

        private async Task PublishUserRegisteredIntegrationEventToMeet(string userId)
        {
            var integrationEvent=new UserRegisteredIntegrationEvent(userId);
            await _integrationEventService.SaveEventAsync(integrationEvent);
            await _integrationEventService.PublishEventsAsync(new[] {integrationEvent.Id});
        }

        [HttpGet]
        [Route("email-confirm")]
        public async Task<IActionResult> ConfirmEmail(string userId,string confirmToken,string expiredTime)
        {
            if(DateTime.Parse(expiredTime)<DateTime.Now)
                return Redirect("http://localhost:3000/auth/confirm-failed?errorType=\"验证码已过期\"");

            var user=await _userManager.FindByIdAsync(userId);
            if(user.EmailConfirmed)
                return Redirect("http://localhost:3000/auth/confirm-failed?errorType=\"已验证，请勿再点击该链接\"");

            var result =await _userManager.ConfirmEmailAsync(user, confirmToken);

            if(result.Errors.Count()>0)
            {
                return Redirect("http://localhost:3000/auth/confirm-failed");
            }

            return Redirect("http://localhost:3000/auth/confirm-succeed");
        }

        [Route("Login")]
        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            return Redirect($"http://localhost:3000/auth/login?returnUrl={returnUrl}");
        }

        /// <summary>
        /// Always,identityserver4 will call this action.
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _loginService.FindByAccount(loginModel.Account, loginModel.AccountType);

                if (user == null)
                {
                    return Unauthorized("Account is not existing");
                }

                await _userStatueService.SetUserStatueFromMySQL(user);//To set user statue from mysql if redis has not the statue of this user of the value is redis is different with value in mysql.

                if (await _loginService.ValidateCredentials(user, loginModel.PassWord))
                {
                    //Actually,this tokenLifetime is the lifetime of cookie
                    //as long as the cookie is not expired.we can get access token by connect/authorize endpoint.
                    var tokenLifetime = _configuration.GetValue("TokenLifetimeMinutes", 24 * 60);

                    var authenticationProps = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(tokenLifetime),
                        AllowRefresh = true,
                        RedirectUri = loginModel.ReturnUrl
                    };

                    if (loginModel.RememberMe)
                    {
                        var permanentCookieLifetime = _configuration.GetValue("PermanentTokenLifetimeDays", 5);

                        authenticationProps.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(permanentCookieLifetime);
                        authenticationProps.IsPersistent = true;
                    }

                    await _loginService.SignInAsync(user, authenticationProps);

                    if (_identityServerInteraction.IsValidReturnUrl(loginModel.ReturnUrl))
                    {
                        return Redirect(loginModel.ReturnUrl);
                    }

                    return Ok();
                }

                return Unauthorized("Account or Password is invalid");
            }

            return BadRequest(await BuildErrorModelStateJsonStr(loginModel, new List<string> { "error model datas." }));
        }
        /// <summary>
        /// 传入的信息json中加入错误信息
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model">post的请求负载对应类型的对象</param>
        /// <param name="errorMessage">错误信息</param>
        /// <returns></returns>
        public Task<JObject> BuildErrorModelStateJsonStr<TModel>(TModel model, IEnumerable<string> errorMessages)
        {
            var modelJson = JObject.FromObject(model!);
            JArray errorArray = new JArray();
            foreach (var error in errorMessages)
            {
                errorArray.Add(error);
            }
            modelJson.Add("errors", errorArray);
            return Task.Run(() =>
            {
                return modelJson;
            });
        }
        /// <summary>
        /// 当通过 end session point 来登出时，IdentityServer应该会默认调用该函数,并带上logoutId。see https://stackoverflow.com/questions/49113792/identityserver4-logout
        /// </summary>
        /// <param name="logoutId">IdentityServer给的</param>
        /// <returns></returns>
        [Route("Logout")]
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            if (User.Identity.IsAuthenticated == false)
            {
                return await Logout(new LogoutModel { LogoutId = logoutId });
            }

            var logoutContext = await _identityServerInteraction.GetLogoutContextAsync(logoutId);
            if (logoutContext.ShowSignoutPrompt == false)//不显示确认登出界面
            {
                return await Logout(new LogoutModel { LogoutId = logoutId });
            }
            //重定向到确认登出界面，带上logoutId，然后确实登出再带上logoutId来post
            return Redirect($"~/Account/LogoutPrompt?logoutId={logoutId}");
        }
        /// <summary>
        /// 具体登出的操作
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout(LogoutModel model)
        {
            var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
            //这种是非本地的Identity提供者，也就是例如用谷歌登陆等，需要创建一个logoutId，IdentityServer应该不会自动生成一个，只有使用本地的验证服务器才会
            //这段是参考 eshopContainer的
            if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
            {
                if (model.LogoutId == null)
                {
                    model.LogoutId = await _identityServerInteraction.CreateLogoutContextAsync();
                }

                string url = "/Account/Logout?logoutId=" + model.LogoutId;
                try
                {
                    await HttpContext.SignOutAsync(idp, new AuthenticationProperties
                    {
                        RedirectUri = url
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "LOGOUT ERROR: {ExceptionMessage}", ex.Message);
                }
            }
            //删除cookie
            await HttpContext.SignOutAsync();
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            //登出后设置User为一个anonymous用户
            HttpContext.User = new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity());

            var logoutContext = await _identityServerInteraction.GetLogoutContextAsync(model.LogoutId);

            //PostLogoutRedirectUri is uri like https://localhost:3000/ instead of domain name like https://localhost:3000 and must be contained in PostLogoutRedirectUris of the client.
            //Or PostLogoutRedirectUri is null;

            if (logoutContext.PostLogoutRedirectUri is null)
                return Redirect("localhost:3000/");

            return Redirect(logoutContext.PostLogoutRedirectUri);
        }
    }
}
