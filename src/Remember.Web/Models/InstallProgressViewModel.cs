using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remember.Web.Models
{
    public class InstallProgressList
    {
        private List<InstallProgress> _list;

        public List<InstallProgress> List { get { return _list; } set { _list = value; } }

        public List<InstallProgressViewModel> ListViewModel
        {
            get
            {
                List<InstallProgressViewModel> rtn = new List<InstallProgressViewModel>();
                foreach (var item in _list)
                {
                    rtn.Add(new InstallProgressViewModel
                    {
                        info = item.info,
                        isSuccess = item.isSuccess
                    });
                }

                return rtn;
            }
        }

        /// <summary>
        /// 当前具有的步骤数
        /// </summary>
        public int Count
        {
            get
            {
                return _list.Count;
            }
        }

        public InstallProgressList()
        {
            _list = new List<InstallProgress>();
        }

        public void AddItem(InstallProgress progress)
        {
            _list.Add(progress);
        }
    }

    public class InstallProgress
    {
        public string info { get; set; }

        /// <summary>
        /// 此步骤是否成功
        /// </summary>
        public bool isSuccess { get; set; }

        /// <summary>
        /// 此步骤发送 Exception 时，将此 Exception 存于此
        /// </summary>
        public Exception exception { get; set; }

        /// <summary>
        /// 是否在此步骤失败时，忽略错误，继续向下执行其它步骤
        /// </summary>
        public bool continueWhenFailure { get; set; }

        public InstallProgress()
        {
            this.isSuccess = false;
            this.continueWhenFailure = false;
        }
    }

    public class InstallProgressViewModel
    {
        public string info { get; set; }

        /// <summary>
        /// 此步骤是否成功
        /// </summary>
        public bool isSuccess { get; set; }
    }
}