﻿using Remember.Domain;
using Remember.Manager;
using Remember.Service;

namespace Remember.Component
{
    public class SysUserComponent : BaseComponent<SysUser, SysUserManager>, SysUserService
    {
    }
}
