using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Models;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        public LoginResult Post([FromBody]LoginViewModel viewModel)
        {
            LoginResult loginResult = new LoginResult();


            return loginResult;
        }
    }
}
