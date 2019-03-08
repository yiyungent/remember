using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remember.Web.Models
{
    public enum InstallResult
    {
        Success,
        Failure
    }

    public class InstallInfoViewModel
    {
        public DateTime InstallTime { get; set; }

        public IList<Exception> Errors { get; set; }

        public InstallResult Result { get; set; }
    }

    public class CurrentProgress
    {
        public int code { get; set; }

        public string currentInfo { get; set; }

        public int currentPos { get; set; }
    }
}