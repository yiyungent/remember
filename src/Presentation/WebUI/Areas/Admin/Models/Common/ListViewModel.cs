using Core;
using Domain;
using Domain.Entities;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebUI.HtmlHelpers;

namespace WebUI.Areas.Admin.Models.Common
{

    public class ListViewModel<T> : ListViewModel
    {
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

        public ListViewModel(IList<T> list, int pageIndex, int pageSize, int totalCount)
        {
            this.List = list;
            this.PageInfo = new PageInfo
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecordCount = totalCount,
                MaxLinkCount = 10
            };
        }
    }

    public class ListViewModel
    {
        public virtual PageInfo PageInfo { get; set; }

        public virtual IList<dynamic> DyList { get; }
    }
}