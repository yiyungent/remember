using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
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

        /// <summary>
        /// 获取媒体文件播放时长
        /// </summary>
        /// <param name="path">媒体文件路径</param>
        /// <returns></returns>
        public string GetMediaTimeLen(string path)
        {
            try
            {

                //Shell32.Shell shell = new Shell32.ShellClass();
                ////文件路径
                //Shell32.Folder folder = shell.NameSpace(path.Substring(0, path.LastIndexOf("\\")));
                ////文件名称
                //Shell32.FolderItem folderitem = folder.ParseName(path.Substring(path.LastIndexOf("\\") + 1));

                //return folder.GetDetailsOf(folderitem, 21);
                return null;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

        /// <summary> 
        /// 获取图片的大小和尺寸 
        /// </summary> 
        /// <param name="imageUrl">图片url</param> 
        /// <param name="size">图片大小 (KB)</param> 
        /// <param name="widthxHeight">图片尺寸（WidthxHeight）</param> 
        public static void GetRemoteImageInfo(string imageUrl, out long size, out string widthxHeight)
        {
            try
            {
                Uri mUri = new Uri(imageUrl);
                HttpWebRequest mRequest = (HttpWebRequest)WebRequest.Create(mUri);
                mRequest.Method = "GET";
                mRequest.Timeout = 200;
                mRequest.ContentType = "text/html;charset=utf-8";
                HttpWebResponse mResponse = (HttpWebResponse)mRequest.GetResponse();
                Stream mStream = mResponse.GetResponseStream();
                size = (mResponse.ContentLength / 1024);
                Image mImage = Image.FromStream(mStream);
                widthxHeight = mImage.Width.ToString() + "x" + mImage.Height.ToString();
                mStream.Close();
            }
            catch (Exception e)
            {
                //MessageBox.Show(aPhotoUrl + "获取失败"); 
                size = 0;
                widthxHeight = "";
            }
        }
    }
}
