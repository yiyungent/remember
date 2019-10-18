using Domain.Entities;
using Services.Core;
using System.Collections.Generic;

namespace Services.Interface
{
    public partial interface IFunctionInfoService : IService<FunctionInfo>
    {
        IList<string> AllAuthKey();
    }
}
