using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public abstract class BaseRepository<T> : IDependency, IRepository<T> where T : class, new()
    {
    }
}
