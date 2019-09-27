using Core;
using Domain;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models.Common;

namespace WebApi.Controllers
{
    public class LogController : ApiController
    {
        public ResponseData AccessLog()
        {
            ResponseData responseData = null;
            try
            {
                Container.Instance.Resolve<LogInfoService>().Create(new LogInfo
                {
                    
                });

                responseData = new ResponseData
                {
                    Code = 1,
                    Message = "写入访问日志成功"
                };
            }
            catch (Exception ex)
            {
                responseData = new ResponseData
                {
                    Code = -1,
                    Message = "写入访问日志失败"
                };
            }

            return responseData;
        }
    }
}
