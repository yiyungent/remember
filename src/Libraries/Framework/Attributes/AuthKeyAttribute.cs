using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Attributes
{
    public class AuthKeyAttribute : Attribute
    {
        public string AuthKey { get; set; }

        public AuthKeyAttribute(string authKey)
        {
            this.AuthKey = authKey;
        }
    }
}
