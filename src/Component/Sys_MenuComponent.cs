using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class Sys_MenuComponent : BaseComponent<Sys_Menu, Sys_MenuManger>, Sys_MenuService
    {
    }
}
