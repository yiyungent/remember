using Common;
using Core;
using Domain;
using Framework.Common;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using WebApi.Attributes;
using WebApi.Infrastructure;
using WebApi.Models;
using WebApi.Models.Common;
using WebApi.Models.UserInfoVM;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/User")]
    public class UserController : ApiController
    {
        #region Get: 获取当前登录账号信息
        [NeedAuth]
        public UserInfoViewModel Get()
        {
            UserInfoViewModel viewModel = null;

            UserInfo userInfo = ApiAccountManager.GetCurrentUserInfo();
            if (userInfo != null)
            {
                viewModel = new UserInfoViewModel()
                {
                    ID = userInfo.ID,
                    UserName = userInfo.UserName,
                    Name = userInfo.Name,
                    Avatar = userInfo.Avatar
                };
            }

            return viewModel;
        }
        #endregion

        #region 更新自己的密码(只能更改当前登录账号的密码)
        [HttpPost]
        [NeedAuth]
        [Route("UpdatePwd")]
        public ResponseData UpdatePwd([FromBody]string oldPassword, [FromBody]string newPassword)
        {
            ResponseData responseData = null;

            UserInfoService userInfoService = Container.Instance.Resolve<UserInfoService>();
            // 当前登录用户
            UserInfo userInfo = userInfoService.Query(new List<ICriterion>
            {
                Expression.Eq("UserName", User.Identity.Name)
            }).FirstOrDefault();
            if (EncryptHelper.MD5Encrypt32(oldPassword) == userInfo.Password)
            {
                userInfo.Password = EncryptHelper.MD5Encrypt32(newPassword);
                userInfoService.Edit(userInfo);

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "密码更新成功"
                };
            }
            else
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "旧密码不正确"
                };
            }

            return responseData;
        }
        #endregion

        #region 注册
        [HttpPost]
        [Route("Register")]
        public RegisterResult Register([FromBody]RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                UserInfoService userInfoService = Container.Instance.Resolve<UserInfoService>();
                bool isExist = userInfoService.Exist(viewModel.UserName?.Trim());
                if (!isExist)
                {
                    userInfoService.Create(new UserInfo()
                    {
                        UserName = viewModel.UserName,
                        Password = EncryptHelper.MD5Encrypt32(viewModel.Password),
                        Name = viewModel.Name.Trim(),
                        Email = viewModel.Email?.Trim()
                    });

                    return new RegisterResult()
                    {
                        Code = 1,
                        Message = "注册成功"
                    };
                }
                else
                {
                    return new RegisterResult()
                    {
                        Code = -1,
                        Message = "该用户名已被注册"
                    };
                }
            }

            return new RegisterResult()
            {
                Code = -2,
                Message = "输入项有误"
            };
        }
        #endregion

        #region 登录
        [HttpPost]
        [Route("Login")]
        public LoginResult Login([FromBody]LoginViewModel viewModel)
        {
            string loginAccount = viewModel.LoginAccount.Trim();
            if (ModelState.IsValid)
            {
                UserInfoService userInfoService = Container.Instance.Resolve<UserInfoService>();
                UserInfo userInfo = null;
                if (IsEmail(loginAccount))
                {
                    // 邮箱
                    userInfo = userInfoService.Query(new List<ICriterion>
                    {
                        Expression.And(
                            Expression.Eq("Email", loginAccount),
                            Expression.Eq("Password", viewModel.Password)
                        )
                    }).FirstOrDefault();
                }
                else if (IsPhone(loginAccount))
                {
                    // 手机号
                    userInfo = userInfoService.Query(new List<ICriterion>
                    {
                        Expression.And(
                            Expression.Eq("Phone", loginAccount),
                            Expression.Eq("Password", viewModel.Password)
                        )
                    }).FirstOrDefault();
                }
                else
                {
                    // 用户名
                    userInfo = userInfoService.Query(new List<ICriterion>
                    {
                        Expression.And(
                            Expression.Eq("UserName", loginAccount),
                            Expression.Eq("Password", viewModel.Password)
                        )
                    }).FirstOrDefault();
                }
                // 账号密码是否正确
                if (userInfo != null)
                {
                    return new LoginResult
                    {
                        Code = 1,
                        Message = "登录成功",
                        ApiToken = JwtHelper.Encode(new JWTokenViewModel
                        {
                            ID = userInfo.ID,
                            UserName = userInfo.UserName,
                            Expire = DateTime.Now.AddDays(7)
                        })
                    };
                }
                else
                {
                    return new LoginResult
                    {
                        Code = -2,
                        Message = "账号或密码错误"
                    };
                }
            }
            else
            {
                return new LoginResult
                {
                    Code = -1,
                    Message = "输入项有误"
                };
            }
        }
        #endregion

        #region Helper

        private bool IsEmail(string str)
        {
            return false;
        }

        private bool IsPhone(string str)
        {
            return false;
        }

        #endregion


    }
}
