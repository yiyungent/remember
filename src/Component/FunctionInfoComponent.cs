using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class FunctionInfoComponent : BaseComponent<FunctionInfo, FunctionInfoManager>, FunctionInfoService
    {
    }
}
