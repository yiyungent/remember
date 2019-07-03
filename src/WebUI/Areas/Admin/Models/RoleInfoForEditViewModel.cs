using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models
{
    public class RoleInfoForEditViewModel
    {
        public int ID { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string Name { get; set; }

        public static explicit operator RoleInfoForEditViewModel(RoleInfo roleInfo)
        {
            RoleInfoForEditViewModel rtnModel = new RoleInfoForEditViewModel
            {
                ID = roleInfo.ID,
                Name = roleInfo.Name
            };

            return rtnModel;
        }
    }
}