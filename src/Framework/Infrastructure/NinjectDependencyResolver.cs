using Framework.Common;
using Framework.Infrastructure.Abstract;
using Framework.Infrastructure.Concrete;
using Framework.Mvc.ViewEngines.Templates;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Framework.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel _kernel;

        public NinjectDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            //_kernel.Bind<IProductRepository>().To<EFProductRepository>();

            //EmailSettings emailSettings = new EmailSettings
            //{
            //    WriteAsFile = bool.Parse(ConfigurationManager
            //        .AppSettings["Email.WriteAsFile"] ?? "false")
            //};

            //_kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
            //    .WithConstructorArgument("settings", emailSettings);

            //_kernel.Bind<IAuthProvider>().To<FormsAuthProvider>();

            _kernel.Bind<IAuthManager>().To<AuthManager>();
            _kernel.Bind<IDBAccessProvider>().To<DBAccessProvider>();
            _kernel.Bind<IWorkContext>().To<WorkContext>();
            _kernel.Bind<ITemplateContext>().To<TemplateContext>();
            _kernel.Bind<ITemplateProvider>().To<TemplateProvider>();
            _kernel.Bind<IWebHelper>().To<WebHelper>().WithConstructorArgument("httpContext", HttpContext.Current);
            _kernel.Bind<WebHelper>().To<WebHelper>().WithConstructorArgument("httpContext", HttpContext.Current);
        }
    }
}
