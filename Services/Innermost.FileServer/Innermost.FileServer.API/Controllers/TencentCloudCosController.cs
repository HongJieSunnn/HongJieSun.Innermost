using Innermost.FileServer.API.Services.TencentCloudCos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Innermost.FileServer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TencentCloudCosController : ControllerBase
    {
        private readonly ITencentClouldCosSTSService _tencentClouldCosSTSService;
        public TencentCloudCosController(ITencentClouldCosSTSService tencentClouldCosSTSService)
        {
            _tencentClouldCosSTSService=tencentClouldCosSTSService;
        }

        [HttpGet]
        [Route("temp-credencial")]
        public async Task<IActionResult> GetTencentCloudCosTemporaryCredentialAsync()
        {
            var temporaryCredential=await _tencentClouldCosSTSService.GetTemporaryCredentialAsync();

            return Ok(temporaryCredential);
        }
    }
}
