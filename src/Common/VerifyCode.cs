using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CaptchaHub.Hub;

namespace Common
{
    public class VerifyCode
    {
        #region 验证码 二次验证
        /// <summary>
        /// 验证码 二次验证
        /// </summary>
        /// <returns>验证通过返回true</returns>
        public static bool SecondVerifyCode(string ticket, string randStr, string ip, out string message)
        {
            bool result = true;
            message = "验证通过";
            ICaptcha captcha = new TencentCaptchaAide("2041300407", "0xeUO8FkVqr8PfJ8GOKZAJA**");
            // 二次验证
            VerifyResult verifyResult = captcha.Verify(ticket, randStr, ip);
            if (verifyResult.Code != 1)
            {
                message = "未通过验证或验证已过期,请重新验证";
                result = false;
            }
            return result;
        }
        #endregion
    }
}
