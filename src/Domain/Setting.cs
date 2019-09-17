using Castle.ActiveRecord;
using Domain.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    [ActiveRecord]
    [Serializable]
    public partial class Setting : BaseEntity<Setting>
    {
    }
}
