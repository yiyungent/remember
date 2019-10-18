using Core.Common.Cache;
using Domain.Entities;
using Repositories.Interface;
using Services.Core;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services.Implement
{
    public partial class FunctionInfoService : BaseService<FunctionInfo>, IFunctionInfoService
    {
        public IList<string> AllAuthKey()
        {
            IList<string> rtn = CacheHelper.Get("AllAuthKey") as IList<string>;
            if (rtn == null)
            {
                IQueryable<FunctionInfo> allFunction = this.All();
                rtn = (from m in allFunction
                       select m.AuthKey.Trim()).ToList();

                CacheHelper.Insert<IList<string>>("AllAuthKey", rtn, DateTime.Now.AddDays(1));
            }

            return rtn;
        }
    }
}
