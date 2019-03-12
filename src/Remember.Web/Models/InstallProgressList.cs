using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remember.Web.Models
{
    public class InstallProgressList
    {
        public List<InstallProgress> List { get; set; }

        public InstallProgressList()
        {
            List = new List<InstallProgress>();
        }

        public void AddItem(InstallProgress progress)
        {
            List.Add(progress);
        }
    }

    public class InstallProgress
    {
        public string info { get; set; }

        public bool isSuccess { get; set; }

        public Exception exception { get; set; }
    }
}