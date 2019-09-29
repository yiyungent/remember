using Core;
using Domain;
using Domain.Base;
using Framework.HtmlHelpers;
using NHibernate.Criterion;
using Service;
using Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.Common
{

    public class ListViewModel<T> : ListViewModel
    //where T : BaseEntity<T>
    {
        public static IList<Order> DefaultOrderList = new List<Order> { new Order("ID", false) };

        public IList<T> List { get; set; }

        public override IList<dynamic> DyList
        {
            get
            {
                IList<dynamic> rtn = new List<dynamic>();
                foreach (T item in this.List)
                {
                    rtn.Add(item);
                }

                return rtn;
            }
        }

        public ListViewModel(IList<ICriterion> queryConditions, IList<Order> orderList, int pageIndex, int pageSize)
        {
            dynamic tempAllList = null;
            // 符合条件的总记录数
            int totalCount = 0;
            switch (typeof(T).ToString())
            {
                case "Domain.UserInfo":
                    tempAllList = Container.Instance.Resolve<UserInfoService>().GetPaged(queryConditions, orderList, pageIndex, pageSize, out totalCount);
                    break;
                case "Domain.RoleInfo":
                    tempAllList = Container.Instance.Resolve<RoleInfoService>().GetPaged(queryConditions, orderList, pageIndex, pageSize, out totalCount);
                    break;
                case "Domain.Article":
                    tempAllList = Container.Instance.Resolve<ArticleService>().GetPaged(queryConditions, orderList, pageIndex, pageSize, out totalCount);
                    break;
                case "Domain.CourseBox":
                    tempAllList = Container.Instance.Resolve<CourseBoxService>().GetPaged(queryConditions, orderList, pageIndex, pageSize, out totalCount);
                    break;
                case "Domain.VideoInfo":
                    tempAllList = Container.Instance.Resolve<VideoInfoService>().GetPaged(queryConditions, orderList, pageIndex, pageSize, out totalCount);
                    break;
                case "Domain.LogInfo":
                    tempAllList = Container.Instance.Resolve<LogInfoService>().GetPaged(queryConditions, orderList, pageIndex, pageSize, out totalCount);
                    break;
            }
            IList<dynamic> allList = tempAllList;
            IList<T> tempList = new List<T>();
            foreach (dynamic item in allList)
            {
                tempList.Add((T)item);
            }

            this.List = tempList;
            this.PageInfo = new PageInfo
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecordCount = totalCount,
                MaxLinkCount = 10
            };
        }

        public ListViewModel(IList<ICriterion> queryConditions, int pageIndex, int pageSize) : this(queryConditions, DefaultOrderList, pageIndex, pageSize)
        { }
    }

    public class ListViewModel
    {
        public virtual PageInfo PageInfo { get; set; }

        public virtual IList<dynamic> DyList { get; }
    }
}