using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using Domain;
using Domain.Entities;
using Framework.Infrastructure.Concrete;
using Services.Interface;

namespace Framework.Mvc.ViewEngines.Templates
{
    public class WorkContext : IWorkContext
    {
        private AuthManager _authManager;

        private int _currentUserId;

        public WorkContext()
        {
            this._authManager = new AuthManager();
            this._currentUserId = AccountManager.GetCurrentAccount().UserId;
        }

        public int CurrentUserId
        {
            get
            {
                return _currentUserId;
            }
        }

        public bool AllowSelectTemplate
        {
            get
            {
                bool isAllow = true;
                isAllow = _authManager.HasAuth("Admin.ThemeTemplate.SelectTemplate");

                return isAllow;
            }
        }

        public string DefaultTemplateName
        {
            get
            {
                return ContainerManager.Resolve<ISettingService>().GetSet("DefaultTemplateName");
            }
        }

        public void SaveSelectedTemplate(string templateName)
        {
            IUserInfoService userInfoService = ContainerManager.Resolve<IUserInfoService>();
            UserInfo userInfo = userInfoService.Find(m => m.ID == _currentUserId && !m.IsDeleted);
            if (userInfo != null)
            {
                userInfo.TemplateName = templateName;
                userInfoService.Update(userInfo);
            }
        }
    }
}
