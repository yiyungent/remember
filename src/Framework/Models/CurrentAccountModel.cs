using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Models
{
    public class CurrentAccountModel
    {
        public UserInfo UserInfo { get; set; }

        public bool IsGuest { get; set; }
    }
}
