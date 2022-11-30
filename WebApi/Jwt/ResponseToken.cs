using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Jwt
{
    public class ResponseToken
    {
        public string access_token { get; set; }
        public string expires_in { get; set; }
        public string token_type { get; set; }

    }
}
