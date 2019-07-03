using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class FileHelper
    {
        #region 直接删除指定目录下的所有文件及文件夹(保留目录)
        /// <summary>
        ///直接删除指定目录下的所有文件及文件夹(保留目录)
        /// </summary>
        /// <param name="dirPath">文件夹路径</param>
        /// <returns>执行结果</returns>
        public static void DeleteDir(string dirPath)
        {
            try
            {
                //去除文件夹和子文件的只读属性
                //去除文件夹的只读属性
                System.IO.DirectoryInfo fileInfo = new DirectoryInfo(dirPath);
                fileInfo.Attributes = FileAttributes.Normal & FileAttributes.Directory;

                //去除文件的只读属性
                System.IO.File.SetAttributes(dirPath, System.IO.FileAttributes.Normal);

                //判断文件夹是否还存在
                if (Directory.Exists(dirPath))
                {
                    foreach (string f in Directory.GetFileSystemEntries(dirPath))
                    {
                        if (File.Exists(f))
                        {
                            //如果有子文件删除文件
                            File.Delete(f);
                            Console.WriteLine(f);
                        }
                        else
                        {
                            //循环递归删除子文件夹
                            DeleteDir(f);
                        }
                    }

                    //删除空文件夹
                    Directory.Delete(dirPath);
                }
            }
            catch (Exception ex) // 异常处理
            {
                Console.WriteLine(ex.Message.ToString());// 异常信息
            }
        }
        #endregion
    }
}
