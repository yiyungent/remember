using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Models.FavoriteVM
{
    public class MyFavListViewModel
    {
        public IList<Group> Groups { get; set; }

        public sealed class Group
        {
            public int ID { get; set; }

            public bool IsFolder { get; set; }

            public string GroupName { get; set; }

            public IList<Favorite> FavList { get; set; }
        }

        public sealed class Favorite
        {
            public int ID { get; set; }

            public string Name { get; set; }

            public int Count { get; set; }

            public bool IsOpen { get; set; }

            public string PicUrl { get; set; }
        }

    }
}