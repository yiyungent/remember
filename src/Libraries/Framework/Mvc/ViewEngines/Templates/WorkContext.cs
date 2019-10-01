using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.Entities;
using Framework.Factories;
using Framework.Infrastructure.Abstract;
using Framework.Infrastructure.Concrete;

namespace Framework.Mvc.ViewEngines.Templates
{
    public class WorkContext : IWorkContext
    {
        private IAuthManager _authManager;
        private IDBAccessProvider _dBAccessProvider;

        public WorkContext(IAuthManager authManager, IDBAccessProvider dBAccessProvider)
        {
            this._authManager = authManager;
            this._dBAccessProvider = dBAccessProvider;
        }

        public UserInfo CurrentUser
        {
            get
            {
                return AccountManager.GetCurrentUserInfo();
            }
        }

        public bool AllowSelectTemplate
        {
            get
            {
                bool isAllow = true;
                isAllow = _authManager.HasAuth("Admin", "ThemeTemplate", "SelectTemplate");

                return isAllow;
            }
        }

        public string DefaultTemplateName
        {
            get
            {
                return _dBAccessProvider.GetSet("DefaultTemplateName");
            }
        }

        public void SaveSelectedTemplate(string templateName)
        {
            _dBAccessProvider.SaveUserTemplateName(templateName);
        }
    }
}
