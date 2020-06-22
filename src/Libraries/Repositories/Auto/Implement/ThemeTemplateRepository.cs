// Code generated by a template
// Project: Remember
// LastUpadteTime: 2020-06-22 05:02:53
using Domain.Entities;
using Repositories.Core;
using Repositories.Interface;

namespace Repositories.Implement
{
    public partial class ThemeTemplateRepository : BaseRepository<ThemeTemplate>, IThemeTemplateRepository
    {
        private readonly RemDbContext _context;

        public ThemeTemplateRepository(RemDbContext context) : base(context)
        {
            this._context = context;
        }
    }
}
