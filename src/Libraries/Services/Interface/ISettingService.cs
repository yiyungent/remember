using Domain.Entities;
using Services.Core;

namespace Services.Interface
{
    public partial interface ISettingService : IService<Setting>
    {
        /// <summary>
        /// 获取设置值，无此设置，返回 null
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetSet(string key);

        /// <summary>
        /// 设置，无此设置，则新建设置
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Set(string key, string value);

        /// <summary>
        /// 发送邮箱验证码-为 找回/重置密码
        /// </summary>
        /// <param name="mail"></param>
        /// <param name="vCode"></param>
        /// <returns></returns>
        bool SendMailVerifyCodeForFindPwd(string mail, out string vCode);
    }
}
