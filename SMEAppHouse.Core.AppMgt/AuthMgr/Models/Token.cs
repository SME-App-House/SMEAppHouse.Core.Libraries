using Newtonsoft.Json;

namespace SMEAppHouse.Core.AppMgt.AuthMgr.Models
{
    public class JsonWebToken
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("user_data")]
        public string UserData { get; set; }
    }
}
