using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Framework.Infrastructure
{
    public class DependencyResolverProvider
    {
        private static NinjectDependencyResolver _instance;

        public static T Get<T>()
        {
            Type type = typeof(T);

            return (T)Get(type);
        }

        public static object Get(Type type)
        {
            if (_instance == null)
            {
                _instance = CreateInstance();
            }

            return _instance.GetService(type);
        }

        private static NinjectDependencyResolver CreateInstance()
        {
            IKernel kernal = new StandardKernel();
            NinjectDependencyResolver dependencyResolver = new NinjectDependencyResolver(kernal);

            return dependencyResolver;
        }
    }
}
