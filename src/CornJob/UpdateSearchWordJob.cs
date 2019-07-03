using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CornJob
{
    public class UpdateSearchWordJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            LogHelper.Log("开始更新搜索热词");
            try
            {
                KeyWordTotalService bll = new KeyWordTotalService();
                bll.DeleteAllKeyWordsRank();
                bll.InsertKeyWordsRank();

                LogHelper.Log("更新完毕");
            }
            catch (Exception ex)
            {
                LogHelper.Log("更新出错: " + ex.Message);
            }

            return Task.CompletedTask;
        }
    }
}
