using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models
{
    public class LoginViewModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }
    }

    public class LoginResult
    {
        public int Code { get; set; }

        public string Message { get; set; }

        public string Token { get; set; }
    }
}