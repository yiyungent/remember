using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Infrastructure;
using WebApi.Models;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {
        public UserInfoViewModel Get()
        {
            UserInfoViewModel viewModel = null;
            UserInfo userInfo = ApiAccountManager.GetCurrentUserInfo();
            if (userInfo != null)
            {
                viewModel = new UserInfoViewModel()
                {
                    ID = userInfo.ID,
                    Name = userInfo.Name,
                    UserName = userInfo.UserName,
                    Avatar = userInfo.Avatar
                };
            }

            return viewModel;
        }
    }
}
