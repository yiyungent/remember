using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                BiliVideoComment.TryVideosComments(length: 500);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Console.ReadKey();
        }
    }
}
