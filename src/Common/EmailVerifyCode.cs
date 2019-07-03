using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web.SessionState;

namespace Common
{
    /// <summary>
    /// 发送邮件验证码行为 原因
    /// </summary>
    public enum SendReason
    {
        /// <summary>
        /// 重置密码
        /// </summary>
        RPwd,
        /// <summary>
        /// 注册
        /// </summary>
        Reg
    }

    public class EmailVerifyCode : IRequiresSessionState
    {
        public static void SendEmailVerifyCode(string email, SendReason sendReason)
        {
            switch (sendReason)
            {
                case SendReason.RPwd:
                    ForResetPassword(email);
                    break;
                case SendReason.Reg:
                    ForRegAccount(email);
                    break;
                default:
                    break;
            }
        }

        private static void ForResetPassword(string email)
        {
            string mailSubject = "【TES】账号安全中心-找回登录密码-" + email + "正在尝试重置密码";
            string mailContent = string.Empty;
            // 生成随机验证码
            Random r = new Random();
            int vCode = r.Next(11111, 99999);

            mailContent += "<p>";
            mailContent += "&nbsp; &nbsp;您正在进行找回登录密码的重置操作，本次请求的邮件验证码是：";
            mailContent += "<strong>" + vCode + "</strong>";
            mailContent += "(为了保证你账号的安全性，请在5分钟内完成设置)。本验证码5分钟内有效，请及时输入。";
            mailContent += "<br><br>";
            mailContent += "&nbsp; &nbsp;为保证账号安全，请勿泄漏此验证码。";
            mailContent += "<br>";
            mailContent += "&nbsp; &nbsp;如非本人操作，及时检查账号或";
            mailContent+= "<a href='#' target='_blank'>联系在线客服</a>";
            mailContent += "<br>";
            mailContent += "&nbsp; &nbsp;祝在【TES】收获愉快！";
            mailContent += "<br><br>";
            mailContent += "&nbsp; &nbsp;（这是一封自动发送的邮件，请不要直接回复）";
            mailContent += "</p>";

            // 保存到Session["vCode"];
            System.Web.HttpContext.Current.Session["vCode"] = vCode;
            Common.SendEmailAide.SendEmail(email, mailSubject, mailContent);
        }

        private static void ForRegAccount(string email)
        {
            string mailSubject = email + "正在注册账号";
            string mailContent = string.Empty;
            // 生成随机验证码
            Random r = new Random();
            int vCode = r.Next(11111, 99999);
            mailContent = "您的验证码:" + vCode;
            // 保存到Session["vCode"];
            System.Web.HttpContext.Current.Session["vCode"] = vCode;
            Common.SendEmailAide.SendEmail(email, mailSubject, mailContent);
        }
    }
}
