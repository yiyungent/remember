using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.UserInfoVM
{
    public class UserInfoViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string UserName { get; set; }

        public string Desc { get; set; }

        public string Avatar { get; set; }
    }
}