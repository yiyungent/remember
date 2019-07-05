using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.RoleInfoVM
{
    public class RoleInfoViewModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }

        public static explicit operator RoleInfoViewModel(RoleInfo roleInfo)
        {
            RoleInfoViewModel rtnModel = new RoleInfoViewModel
            {
                ID = roleInfo.ID,
                Name = roleInfo.Name
            };

            return rtnModel;
        }
    }
}