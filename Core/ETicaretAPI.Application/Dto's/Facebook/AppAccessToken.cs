using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Dto_s.Facebook
{
    public class AppAccessToken
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
    }
}

//Example Json Format
//"{\"access_token\":\"415584320680804|JOf8TrOiSSLbVsR-6C0wmSyAck0\",\"token_type\":\"bearer\"}"
