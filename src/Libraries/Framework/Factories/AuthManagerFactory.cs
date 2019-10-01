using Framework.Infrastructure;
using Framework.Infrastructure.Abstract;
using Framework.Infrastructure.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Factories
{
    public class AuthManagerFactory
    {
        private static IAuthManager _instance;

        public static IAuthManager Get()
        {
            if (_instance != null)
            {
                return _instance;
            }

            //IAuthManager authManager = new AuthManager(new DBAccessProvider());
            IAuthManager authManager = DependencyResolverProvider.Get<IAuthManager>();
            _instance = authManager;

            return _instance;
        }
    }
}
