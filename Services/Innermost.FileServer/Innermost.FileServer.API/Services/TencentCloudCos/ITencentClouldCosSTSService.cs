namespace Innermost.FileServer.API.Services.TencentCloudCos
{
    public interface ITencentClouldCosSTSService
    {
        /// <summary>
        /// Get temporary credential for front-end app to upload file.
        /// </summary>
        /// <returns>Credential dictionary.Demo:https://github.com/tencentyun/qcloud-cos-sts-sdk/blob/master/dotnet/demo/Program.cs</returns>
        Task<Dictionary<string, object>> GetTemporaryCredentialAsync();
    }
}
