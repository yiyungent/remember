﻿using Domain.Entities;
using Services.Core;
using Services.Interface;
using System;
using System.Collections.Generic;
using Core.Common;
using System.Linq;
using Core.Common.Cache;

namespace Services.Implement
{
    public partial class SettingService : BaseService<Setting>, ISettingService
    {
        /// <summary>
        /// 获取设置值，无此设置，返回 null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetSet(string key)
        {
            string value = CacheHelper.Get<string>("Settings." + key);
            try
            {
                if (value == null)
                {
                    value = this._repository.Find(m => m.SetKey == key).SetValue;

                    CacheHelper.Insert<string>("Settings." + key, value, DateTime.Now.AddDays(1));
                }
            }
            catch (Exception ex)
            { }

            return value;
        }

        /// <summary>
        /// 设置，无此设置，则新建设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Set(string key, string value)
        {
            Setting dbModel = null;
            try
            {
                dbModel = this._repository.Find(m => m.SetKey == key);
            }
            catch (Exception ex)
            { }
            if (dbModel == null)
            {
                // 创建
                dbModel = new Setting();
                dbModel.SetKey = key;
                dbModel.SetValue = value;
                this._repository.Create(dbModel);
            }
            else
            {
                // 更新
                dbModel.SetValue = value;
                this._repository.Update(dbModel);
            }
            this._repository.SaveChanges();

            // 更新缓存
            CacheHelper.Insert<string>("Settings." + key, value, DateTime.Now.AddDays(1));
        }

        #region 发送邮箱验证码-为 找回/重置密码
        public bool SendMailVerifyCodeForFindPwd(string mail, out string vCode)
        {
            try
            {
                // 生成随机验证码
                Random r = new Random();
                vCode = r.Next(11111, 99999).ToString();

                // TODO: 这么筛选，可能存在问题，也可以在之后再去除被删除的。现在已改为之后再去掉被删除的
                // 获取所有需要用到的设置项
                IList<Setting> settings = this._repository.Filter(m =>
                    m.SetKey == "FindPwd.Mail.Subject"
                    || m.SetKey == "FindPwd.Mail.Content"
                    || m.SetKey == "Mail.DisplayAddress"
                    || m.SetKey == "Mail.DisplayAddress"
                    || m.SetKey == "Mail.UserName"
                    || m.SetKey == "Mail.Password"
                    || m.SetKey == "Smtp.Host"
                    || m.SetKey == "Smtp.Port"
                    || m.SetKey == "Smtp.EnableSsl"
                ).ToList();

                string mailSubjectTemplate = settings.FirstOrDefault(m => m.SetKey == "FindPwd.Mail.Subject")?.SetValue ?? "";
                string mailContentTemplate = settings.FirstOrDefault(m => m.SetKey == "FindPwd.Mail.Content")?.SetValue ?? "";

                string mailSubject = ReplaceMailTemplate(mailSubjectTemplate, mail, vCode);
                string mailContent = ReplaceMailTemplate(mailContentTemplate, mail, vCode);

                MailOptions mailOptions = new MailOptions();
                mailOptions.SenderDisplayAddress = settings.FirstOrDefault(m => m.SetKey == "Mail.DisplayAddress")?.SetValue ?? "";
                mailOptions.SenderDisplayName = settings.FirstOrDefault(m => m.SetKey == "Mail.DisplayAddress")?.SetValue ?? "";
                mailOptions.UserName = settings.FirstOrDefault(m => m.SetKey == "Mail.UserName")?.SetValue ?? "";
                mailOptions.Password = settings.FirstOrDefault(m => m.SetKey == "Mail.Password")?.SetValue ?? "";
                mailOptions.Subject = mailSubject;
                mailOptions.Content = mailContent;
                mailOptions.ReceiveAddress = mail;
                mailOptions.Host = settings.FirstOrDefault(m => m.SetKey == "Smtp.Host")?.SetValue ?? "";
                mailOptions.Port = Convert.ToInt32(settings.FirstOrDefault(m => m.SetKey == "Smtp.Port")?.SetValue ?? "25");
                string enableSsl = settings.FirstOrDefault(m => m.SetKey == "Smtp.EnableSsl")?.SetValue ?? "";
                if (enableSsl == "" || enableSsl == "0")
                {
                    mailOptions.EnableSsl = false;
                }

                bool isSuccess = MailHelper.SendMail(mailOptions, out string errorMsg);

                return isSuccess;
            }
            catch (System.Exception ex)
            {
                vCode = null;
                return false;
            }
        }
        #endregion

        #region 发送邮箱验证码-为 注册账号
        public bool SendMailVerifyCodeForRegAccount(string email)
        {
            try
            {
                // TODO: 发送邮箱验证码-为 注册账号
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }
        #endregion


        #region 解析邮件模板

        private string ReplaceMailTemplate(string template, string receiveMail, string vCode)
        {
            string rtnStr = "";

            // 获取所有需要用到的设置项
            IList<Setting> settings = this._repository.Filter(m =>
                m.SetKey == "Web.Name"
                || m.SetKey == "Mail.DisplayAddress"
                || m.SetKey == "Mail.DisplayAddress"
                || m.SetKey == "Mail.UserName"
            ).ToList();
            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            keyValues.Add("Web.Name", settings.FirstOrDefault(m => m.SetKey == "Web.Name")?.SetValue ?? "");

            keyValues.Add("Mail.DisplayAddress", settings.FirstOrDefault(m => m.SetKey == "Mail.DisplayAddress")?.SetValue ?? "");
            keyValues.Add("Mail.DisplayAddress", settings.FirstOrDefault(m => m.SetKey == "Mail.DisplayAddress")?.SetValue ?? "");
            keyValues.Add("Mail.UserName", settings.FirstOrDefault(m => m.SetKey == "Mail.UserName")?.SetValue ?? "");
            keyValues.Add("ReceiveMail", receiveMail);
            keyValues.Add("VCode", vCode);

            if (!string.IsNullOrEmpty(template))
            {
                rtnStr = template;
                foreach (var item in keyValues)
                {
                    if (rtnStr.Contains(item.Key))
                    {
                        rtnStr = rtnStr.Replace("{{" + item.Key + "}}", item.Value);
                    }
                }
            }

            return rtnStr;
        }

        #endregion

    }
}
