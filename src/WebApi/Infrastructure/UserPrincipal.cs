using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace WebApi.Infrastructure
{
    public class UserPrincipal : IPrincipal
    {
        /// <summary>
        /// Gets the identity of the current principal.
        /// </summary>
        public IIdentity Identity { get; }

        /// <summary>
        /// Determines whether the current principal belongs to the specified role.
        /// </summary>
        /// <param name="role">The name of the role for which to check membership.</param>
        /// <returns>true if the current principal is a member of the specified role; otherwise, false.</returns>
        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        #region Ctor
        public UserPrincipal(IIdentity identity)
        {
            this.Identity = identity;
        }
        #endregion
    }
}