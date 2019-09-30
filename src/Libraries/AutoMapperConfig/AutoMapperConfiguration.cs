using AutoMapper;
using Domain.Entities;
using ViewModel.Article;

namespace AutoMapperConfig
{
    /// <summary>
    /// AutoMapper的全局实体映射配置静态类
    /// </summary>
    public static class AutoMapperConfiguration
    {
        public static void Init()
        {
            MapperConfiguration = new MapperConfiguration(cfg =>
            {

                #region Article
                //将领域实体映射到视图实体
                cfg.CreateMap<Article, ArticleViewModel>()
                    .ForMember(d => d.ID, d => d.MapFrom(s => s.ID))
                ;
                //将视图实体映射到领域实体
                cfg.CreateMap<ArticleViewModel, Article>();
                #endregion

            });

            Mapper = MapperConfiguration.CreateMapper();
        }

        public static IMapper Mapper { get; private set; }

        public static MapperConfiguration MapperConfiguration { get; private set; }
    }
}