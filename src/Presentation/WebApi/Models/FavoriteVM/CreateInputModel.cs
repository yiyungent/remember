using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.FavoriteVM
{
    public class CreateInputModel
    {
        public string Name { get; set; }

        public string Desc { get; set; }

        public bool IsOpen { get; set; }
    }
}