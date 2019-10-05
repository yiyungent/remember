using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace WebApi.Infrastructure
{
    public class UserIdentity : IIdentity
    {
        public int ID { get; }

        public string Name { get; }

        public string AuthenticationType { get; }

        public bool IsAuthenticated { get; }

        #region Ctor
        public UserIdentity(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        } 
        #endregion
    }
}