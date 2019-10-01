using Domain.Entities;
using Services.Core;
using Services.Interface;
using System;
using System.Collections.Generic;
using Core.Common;
using System.Linq;

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
            string value = null;
            try
            {
                value = this._repository.Find(m => m.SetKey == key && !m.IsDeleted).SetValue;
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
                dbModel = this._repository.Find(m => m.SetKey == key && !m.IsDeleted);
            }
            catch (Exception ex)
            { }
            if (dbModel == null)
            {
                // 创建
                dbModel = new Setting();
                dbModel.SetKey = key;
                dbModel.SetValue = value;
                dbModel.IsDeleted = false;
                this._repository.Create(dbModel);
            }
            else
            {
                // 更新
                dbModel.SetValue = value;
                this._repository.Update(dbModel);
            }
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
                    m.SetKey == "FindPwd_MailSubject"
                    || m.SetKey == "FindPwd_MailContent"
                    || m.SetKey == "MailDisplayAddress"
                    || m.SetKey == "MailDisplayName"
                    || m.SetKey == "MailUserName"
                    || m.SetKey == "MailPassword"
                    || m.SetKey == "StmpHost"
                    || m.SetKey == "StmpPort"
                    || m.SetKey == "SmtpEnableSsl"
                ).Where(m => !m.IsDeleted).ToList();

                string mailSubjectTemplate = settings.FirstOrDefault(m => m.SetKey == "FindPwd_MailSubject")?.SetValue ?? "";
                string mailContentTemplate = settings.FirstOrDefault(m => m.SetKey == "FindPwd_MailContent")?.SetValue ?? "";

                string mailSubject = ReplaceMailTemplate(mailSubjectTemplate, mail, vCode);
                string mailContent = ReplaceMailTemplate(mailContentTemplate, mail, vCode);

                MailOptions mailOptions = new MailOptions();
                mailOptions.SenderDisplayAddress = settings.FirstOrDefault(m => m.SetKey == "MailDisplayAddress")?.SetValue ?? ""; //GetSet("MailDisplayAddress");
                mailOptions.SenderDisplayName = settings.FirstOrDefault(m => m.SetKey == "MailDisplayName")?.SetValue ?? "";//GetSet("MailDisplayName");
                mailOptions.UserName = settings.FirstOrDefault(m => m.SetKey == "MailUserName")?.SetValue ?? "";//GetSet("MailUserName");
                mailOptions.Password = settings.FirstOrDefault(m => m.SetKey == "MailPassword")?.SetValue ?? "";//GetSet("MailPassword");
                mailOptions.Subject = mailSubject;
                mailOptions.Content = mailContent;
                mailOptions.ReceiveAddress = mail;
                mailOptions.Host = settings.FirstOrDefault(m => m.SetKey == "StmpHost")?.SetValue ?? ""; //GetSet("StmpHost");
                //mailOptions.Port = Convert.ToInt32(GetSet("StmpPort"));
                mailOptions.Port = Convert.ToInt32(settings.FirstOrDefault(m => m.SetKey == "StmpPort")?.SetValue ?? "25");
                string enableSsl = settings.FirstOrDefault(m => m.SetKey == "SmtpEnableSsl")?.SetValue ?? "";//GetSet("SmtpEnableSsl");
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
                m.SetKey == "WebUITitle"
                || m.SetKey == "WebUIDesc"
                || m.SetKey == "WebUISite"
                || m.SetKey == "MailDisplayAddress"
                || m.SetKey == "MailDisplayName"
                || m.SetKey == "MailUserName"
            ).Where(m => !m.IsDeleted).ToList();
            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            //keyValues.Add("WebUITitle", GetSet("WebUITitle"));
            keyValues.Add("WebUITitle", settings.FirstOrDefault(m => m.SetKey == "WebUITitle")?.SetValue ?? "");
            //keyValues.Add("WebUIDesc", GetSet("WebUIDesc"));
            keyValues.Add("WebUIDesc", settings.FirstOrDefault(m => m.SetKey == "WebUIDesc")?.SetValue ?? "");
            //keyValues.Add("WebUISite", GetSet("WebUISite"));
            keyValues.Add("WebUISite", settings.FirstOrDefault(m => m.SetKey == "WebUISite")?.SetValue ?? "");

            //keyValues.Add("MailDisplayAddress", GetSet("MailDisplayAddress"));
            keyValues.Add("MailDisplayAddress", settings.FirstOrDefault(m => m.SetKey == "MailDisplayAddress")?.SetValue ?? "");
            //keyValues.Add("MailDisplayName", GetSet("MailDisplayName"));
            keyValues.Add("MailDisplayName", settings.FirstOrDefault(m => m.SetKey == "MailDisplayName")?.SetValue ?? "");
            //keyValues.Add("MailUserName", GetSet("MailUserName"));
            keyValues.Add("MailUserName", settings.FirstOrDefault(m => m.SetKey == "MailUserName")?.SetValue ?? "");
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
