using Domain.Entities;
using ViewModel.Article;

namespace AutoMapperConfig
{
    /// <summary>
    /// 数据库表-实体映射静态扩展类
    /// </summary>
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        #region Article

        public static ArticleViewModel ToModel(this Article entity)
        {
            return entity.MapTo<Article, ArticleViewModel>();
        }

        public static Article ToEntity(this ArticleViewModel model)
        {
            return model.MapTo<ArticleViewModel, Article>();
        }

        #endregion

    }
}