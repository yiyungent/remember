using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CornJob
{
    public class UpdateSearchWordsJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            File.AppendAllText(@"C:\Users\lenovo\Desktop\log.txt", DateTime.Now.ToString() + Environment.NewLine);
            try
            {
                KeyWordsTotalService bll = new KeyWordsTotalService();
                bll.DeleteAllKeyWordsRank();
                bll.InsertKeyWordsRank();
            }
            catch (Exception ex)
            {
                File.AppendAllText(@"C:\Users\lenovo\Desktop\log.txt", DateTime.Now.ToString() + Environment.NewLine + ex.Message + Environment.NewLine);
            }


            return Task.CompletedTask;
        }
    }
}
