using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Dto_s.Facebook
{
    public class UserAccessTokenValidation
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("is_valid")]
        public bool IsValid { get; set; }
    }
}

//Example Json Format
//"{\"data\":{\"app_id\":\"415584320680804\",\"type\":\"USER\",\"application\":\"Mini E-Ticaret\",\" +
//    ""data_access_expires_at\":1671819752,\"expires_at\":1664049600,\"is_valid" +
//    "\":true,\"scopes\":[\"email\",\"public_profile\"],\"user_id\":\"1146686699585918\"}}"
