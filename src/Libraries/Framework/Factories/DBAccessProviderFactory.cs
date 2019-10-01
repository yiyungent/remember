using Framework.Infrastructure;
using Framework.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Factories
{
    public class DBAccessProviderFactory
    {
        private static IDBAccessProvider _instance;

        public static IDBAccessProvider Get()
        {
            if (_instance != null)
            {
                return _instance;
            }

            IDBAccessProvider _dBAccessProvider = DependencyResolverProvider.Get<IDBAccessProvider>();
            _instance = _dBAccessProvider;

            return _instance;
        }
    }
}
