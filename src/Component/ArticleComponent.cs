using Component.Base;
using Domain;
using Manager;
using Service;

namespace Component
{
    public class ArticleComponent : BaseComponent<Article, ArticleManager>, ArticleService
    {
    }
}
