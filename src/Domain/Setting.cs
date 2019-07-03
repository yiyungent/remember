using Castle.ActiveRecord;
using Domain.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Domain
{
    [ActiveRecord]
    public partial class Setting : BaseEntity<Setting>
    {
    }
}
