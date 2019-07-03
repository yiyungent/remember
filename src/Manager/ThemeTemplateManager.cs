using Domain;
using Manager.Base;
using NHibernate.Criterion;

namespace Manager
{
    public class ThemeTemplateManager : BaseManager<ThemeTemplate>
    {
        public bool Exist(string templateName, int exceptId = 0)
        {
            bool isExist = Count(Expression.And(
                                Expression.Eq("TemplateName", templateName),
                                Expression.Not(Expression.Eq("ID", exceptId))
                            )) > 0;

            return isExist;
        }
    }
}
