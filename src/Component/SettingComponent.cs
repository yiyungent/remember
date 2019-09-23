using Component.Base;
using Domain;
using Manager;
using NHibernate.Criterion;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Component
{
    public class SettingComponent : BaseComponent<Setting, SettingManager>, SettingService
    {
        public string GetSet(string key)
        {
            return _manager.GetSet(key);
        }

        public void Set(string key, string value)
        {
            _manager.Set(key, value);
        }

        #region 发送邮箱验证码-为 找回/重置密码
        public bool SendMailVerifyCodeForFindPwd(string mail, out string vCode)
        {
            try
            {
                // 生成随机验证码
                Random r = new Random();
                vCode = r.Next(11111, 99999).ToString();

                string mailSubjectTemplate = _manager.Query(new List<ICriterion>
                {
                    Expression.Eq("SetKey", "FindPwd_MailSubject")
                }).FirstOrDefault()?.SetValue ?? "";
                string mailContentTemplate = _manager.Query(new List<ICriterion>
                {
                    Expression.Eq("SetKey", "FindPwd_MailContent")
                }).FirstOrDefault()?.SetValue ?? "";

                string mailSubject = ReplaceMailTemplate(mailSubjectTemplate, mail, vCode);
                string mailContent = ReplaceMailTemplate(mailContentTemplate, mail, vCode);

                Common.MailOptions mailOptions = new Common.MailOptions();
                mailOptions.SenderDisplayAddress = GetSet("MailDisplayAddress");
                mailOptions.SenderDisplayName = GetSet("MailDisplayName");
                mailOptions.UserName = GetSet("MailUserName");
                mailOptions.Password = GetSet("MailPassword");
                mailOptions.Subject = mailSubject;
                mailOptions.Content = mailContent;
                mailOptions.ReceiveAddress = mail;
                mailOptions.Host = GetSet("StmpHost");
                mailOptions.Port = Convert.ToInt32(GetSet("StmpPort"));
                string enableSsl = GetSet("SmtpEnableSsl");
                if (enableSsl == "" || enableSsl == "0")
                {
                    mailOptions.EnableSsl = false;
                }

                bool isSuccess = Common.MailHelper.SendMail(mailOptions, out string errorMsg);

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

            Dictionary<string, string> keyValues = new Dictionary<string, string>();

            keyValues.Add("WebUITitle", GetSet("WebUITitle"));
            keyValues.Add("WebUIDesc", GetSet("WebUIDesc"));
            keyValues.Add("WebUISite", GetSet("WebUISite"));

            keyValues.Add("MailDisplayAddress", GetSet("MailDisplayAddress"));
            keyValues.Add("MailDisplayName", GetSet("MailDisplayName"));
            keyValues.Add("MailUserName", GetSet("MailUserName"));
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
