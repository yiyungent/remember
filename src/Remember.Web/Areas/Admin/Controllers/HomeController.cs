using NHibernate.Criterion;
using Remember.Core;
using Remember.Domain;
using Remember.Service;
using Remember.Web.Attributes;
using Remember.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Remember.Web.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        #region 后台首页
        public ViewResult Index()
        {
            IList<SysMenu> allMenuList = GetMenuAuthForLoginUser();
            ViewBag.AllMenuList = allMenuList;

            return View();
        }
        #endregion

        #region 后台登录
        [HttpGet]
        [NeedlessAuth]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        [NeedlessAuth]
        public JsonResult Login(SysUser model)
        {
            // 检查是否存在此用户
            IList<SysUser> findUserList = Container.Instance.Resolve<SysUserService>().Query(new List<ICriterion>
            {
                Expression.Eq("LoginAccount", model.LoginAccount)
            });

            if (findUserList == null || findUserList.Count == 0)
            {
                return Json(new { code = -1, message = "账号不存在" });
            }

            // 检查账号密码是否正确
            bool isRight = false;
            bool canStatus = false;
            SysUser loginUser = null;
            foreach (SysUser user in findUserList)
            {
                if (user.Password == Common.StringHelper.EncodeMD5(model.Password))
                {
                    isRight = true;
                    if (user.Status == 0)
                    {
                        canStatus = true;
                        loginUser = user;
                        break;
                    }
                }
            }
            if (!isRight)
            {
                return Json(new { code = -2, message = "账号或密码错误" });
            }
            if (!canStatus)
            {
                return Json(new { code = -3, message = "账号被禁用" });
            }

            Session["loginUser"] = loginUser;
            Session.Timeout = 120;

            return Json(new { code = 1, message = "登录成功" });
        }
        #endregion

        #region 辅助方法
        private IList<SysMenu> GetMenuAuthForLoginUser()
        {
            IList<SysMenu> rtn = new List<SysMenu>();
            SysUser loginUser = Session["loginUser"] as SysUser;
            loginUser = loginUser ?? new SysUser { SysRoleList = new List<SysRole>() };

            foreach (SysRole role in loginUser.SysRoleList)
            {
                foreach (SysMenu menu in role.SysMenuList)
                {
                    if (!rtn.Contains(menu, new SysMenuCompare()))
                    {
                        rtn.Add(menu);
                    }
                }
            }

            return rtn;
        }
        #endregion

        #region SysMenu ID比较器
        /// <summary>
        /// SysMenu 比较器----只比较 ID
        /// </summary>
        sealed class SysMenuCompare : IEqualityComparer<SysMenu>
        {
            public bool Equals(SysMenu x, SysMenu y)
            {
                if (x == null && y == null)
                {
                    return true;
                }
                if (x == null ^ y == null)
                {
                    return false;
                }
                return x.ID == y.ID;
            }

            public int GetHashCode(SysMenu obj)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region SysUser 账号密码比较器
        sealed class SysUserLoginAccountAndPwdCompare : IEqualityComparer<SysUser>
        {
            public bool Equals(SysUser x, SysUser y)
            {
                if (x == null || y == null) return false;
                return x.LoginAccount == y.LoginAccount && x.Password == y.Password;
            }

            public int GetHashCode(SysUser obj)
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
