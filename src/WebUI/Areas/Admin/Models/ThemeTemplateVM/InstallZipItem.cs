using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.ThemeTemplateVM
{
    public class InstallZipItem
    {
        /// <summary>
        /// 模板名
        /// </summary>
        public string TemplateName { get; set; }

        /// <summary>
        /// 文件名称（包含绝对物理路径）
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 文件名称（包含后缀名）
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// 文件版本
        /// </summary>
        public string FileVersion { get; set; }

        /// <summary>
        /// 产品版本
        /// </summary>
        public string ProductVersion { get; set; }

        /// <summary>
        /// 系统显示文件版本
        /// 通常版本号显示为「主版本号.次版本号.生成号.专用部件号」
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 文件说明
        /// </summary>
        public string FileDescription { get; set; }

        /// <summary>
        /// 文件语言
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// 原始文件名称
        /// </summary>
        public string OriginalFilename { get; set; }

        /// <summary>
        /// 文件版权
        /// </summary>
        public string LegalCopyright { get; set; }

        /// <summary>
        /// 文件大小（KB）
        /// </summary>
        public double Length { get; set; }

        public InstallZipItem(FileVersionInfo fileVersionInfo, FileInfo fileInfo)
        {
            if (fileVersionInfo == null || fileInfo == null)
            {
                throw new Exception("模板安装包文件不存在");
            }
            this.TemplateName = fileInfo.Name.Remove(fileInfo.Name.LastIndexOf('.'));
            this.FilePath = fileVersionInfo.FileName;
            this.FileName = fileInfo.Name;
            this.ProductName = fileVersionInfo.ProductName;
            this.CompanyName = fileVersionInfo.CompanyName;
            this.FileVersion = fileVersionInfo.FileVersion;
            this.ProductVersion = fileVersionInfo.ProductVersion;
            this.Version = fileVersionInfo.ProductMajorPart + '.' + fileVersionInfo.ProductMinorPart + '.' + fileVersionInfo.ProductBuildPart + '.' + fileVersionInfo.ProductPrivatePart.ToString();
            this.FileDescription = fileVersionInfo.FileDescription;
            this.Language = fileVersionInfo.Language;
            this.OriginalFilename = fileVersionInfo.OriginalFilename;
            this.LegalCopyright = fileVersionInfo.LegalCopyright;
            this.Length = System.Math.Ceiling(fileInfo.Length / 1024.0);
        }

        public InstallZipItem(string zipFilePath) : this(FileVersionInfo.GetVersionInfo(zipFilePath), new FileInfo(zipFilePath))
        { }
    }
}