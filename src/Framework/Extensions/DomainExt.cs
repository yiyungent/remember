using Core;
using Domain;
using Framework.Factories;
using Framework.Infrastructure.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Extensions
{
    public static class DomainExt
    {
        public static IList<FunctionInfo> FunctionInfoList(this Sys_Menu sys_Menu)
        {
            IList<FunctionInfo> rtn = null;
            IDBAccessProvider dBAccessProvider = HttpOneRequestFactory.Get<IDBAccessProvider>();
            int menuId = sys_Menu.ID;
            rtn = dBAccessProvider.GetFunctionListBySys_MenuId(menuId);

            return rtn;
        }

    }
}
