using COSSTS;

namespace Innermost.FileServer.API.Services.TencentCloudCos
{
    public class TencentClouldCosSTSService : ITencentClouldCosSTSService
    {
        private readonly Dictionary<string, object> _values;
        public TencentClouldCosSTSService(string bucket, string region, string[] allowPrefixes, string[] allowActions, int durationSeconds, string secretId, string secretKey)
        {
            _values = new Dictionary<string, object>()
            {
                {nameof(bucket),bucket },
                {nameof(region),region },
                {nameof(allowPrefixes),allowPrefixes },
                {nameof(allowActions),allowActions },
                {nameof(durationSeconds),durationSeconds},
                {nameof(secretId),secretId },
                {nameof(secretKey),secretKey },
            };
        }
        public Task<Dictionary<string, object>> GetTemporaryCredentialAsync()
        {
            Dictionary<string, object> credential = STSClient.genCredential(_values);

            return Task.FromResult(credential);
        }
    }
}
