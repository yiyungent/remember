using Core;
using Domain;
using Domain.Entities;
using Framework.Common;
using Framework.Mvc.ViewEngines.Templates;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebUI.HtmlHelpers;

namespace WebUI.Areas.Admin.Models.ThemeTemplateVM
{
    public class ThemeTemplateListViewModel
    {
        public IList<dynamic> List { get; set; }

        public PageInfo PageInfo { get; set; }

        public ThemeTemplateListViewModel(Expression<Func<ThemeTemplate, bool>> filter, int pageIndex, int pageSize, HttpContextBase httpContextBase, string cat = "open")
        {
            //IList<ThemeTemplate> installedTemplateList = Container.Instance.Resolve<ThemeTemplateService>().GetPaged(queryConditions, orderList, pageIndex, pageSize, out int totalCount);
            IList<ThemeTemplate> installedTemplateList = ContainerManager.Resolve<IThemeTemplateService>().Filter<int>(pageIndex, pageSize, out int totalCount, filter, m => m.ID, false).ToList();
            //string defaultTemplateName = Container.Instance.Resolve<SettingService>().GetSet("DefaultTemplateName") ?? "";
            string defaultTemplateName = ContainerManager.Resolve<ISettingService>().GetSet("DefaultTemplateName");
            IList<string> installedTemplateNames = installedTemplateList.Select(m => m.TemplateName).ToList();
            ITemplateProvider templateProvider = new TemplateProvider(new WebHelper(httpContextBase));
            IList<TemplateConfiguration> templateConfigurations = templateProvider.GetTemplateConfigurations();

            IList<dynamic> list = new List<dynamic>();
            switch (cat.ToLower())
            {
                case "open":                // 启用---注意:启用，一定已安装
                    // 数据库中存放的已安装模板 被标记为 启用 的记录
                    IList<string> openTemplateNames = installedTemplateList.Where(m => m.IsOpen == 1).Select(m => m.TemplateName).ToList();
                    foreach (var templateName in openTemplateNames)
                    {
                        OpenCloseItem openItem = new OpenCloseItem();
                        TemplateConfiguration templateConfiguration = templateConfigurations.Where(m => m.TemplateName.ToLower() == templateName.ToLower()).FirstOrDefault();
                        if (templateConfiguration == null)
                        {
                            // 数据库中存在此模板记录，但模板目录却没有对应模板配置文件
                            continue;
                        }
                        openItem.TemplateName = templateConfiguration.TemplateName;
                        openItem.Title = templateConfiguration.Title;
                        openItem.Authors = templateConfiguration.Authors;
                        openItem.Description = templateConfiguration.Description;
                        openItem.PreviewImageUrl = templateConfiguration.PreviewImageUrl;
                        openItem.IsDefault = templateName.ToLower() == defaultTemplateName.ToLower();
                        openItem.Status = installedTemplateList.Where(m => m.TemplateName.ToLower() == templateName.ToLower()).Select(m => m.IsOpen).FirstOrDefault();
                        openItem.ID = installedTemplateList.Where(m => m.TemplateName.ToLower() == templateName.ToLower()).Select(m => m.ID).FirstOrDefault();

                        list.Add(openItem);
                    }
                    break;
                case "close":               // 禁用---注意：禁用，一定已安装
                    // 数据库中存放的已安装模板 被标记为 禁用 的记录
                    IList<string> closeTemplateNames = installedTemplateList.Where(m => m.IsOpen == 0).Select(m => m.TemplateName).ToList();
                    foreach (var templateName in closeTemplateNames)
                    {
                        OpenCloseItem closeItem = new OpenCloseItem();
                        TemplateConfiguration templateConfiguration = templateConfigurations.Where(m => m.TemplateName.ToLower() == templateName.ToLower()).FirstOrDefault();
                        if (templateConfiguration == null)
                        {
                            // 数据库中存在此模板记录，但模板目录却没有对应模板配置文件
                            continue;
                        }
                        closeItem.TemplateName = templateConfiguration.TemplateName;
                        closeItem.Title = templateConfiguration.Title;
                        closeItem.Authors = templateConfiguration.Authors;
                        closeItem.Description = templateConfiguration.Description;
                        closeItem.PreviewImageUrl = templateConfiguration.PreviewImageUrl;
                        closeItem.IsDefault = templateName.ToLower() == defaultTemplateName.ToLower();
                        closeItem.Status = installedTemplateList.Where(m => m.TemplateName.ToLower() == templateName.ToLower()).Select(m => m.IsOpen).FirstOrDefault();
                        closeItem.ID = installedTemplateList.Where(m => m.TemplateName.ToLower() == templateName.ToLower()).Select(m => m.ID).FirstOrDefault();

                        list.Add(closeItem);
                    }
                    break;
                case "installed":           // 已安装
                    // 数据库中存放的已安装模板的记录
                    foreach (var templateName in installedTemplateNames)
                    {
                        OpenCloseItem opencloseItem = new OpenCloseItem();
                        TemplateConfiguration templateConfiguration = templateConfigurations.Where(m => m.TemplateName.ToLower() == templateName.ToLower()).FirstOrDefault();
                        if (templateConfiguration == null)
                        {
                            // 数据库中存在此模板记录，但模板目录却没有对应模板配置文件
                            continue;
                        }
                        opencloseItem.TemplateName = templateConfiguration.TemplateName;
                        opencloseItem.Title = templateConfiguration.Title;
                        opencloseItem.Authors = templateConfiguration.Authors;
                        opencloseItem.Description = templateConfiguration.Description;
                        opencloseItem.PreviewImageUrl = templateConfiguration.PreviewImageUrl;
                        opencloseItem.IsDefault = templateName.ToLower() == defaultTemplateName.ToLower();
                        opencloseItem.Status = installedTemplateList.Where(m => m.TemplateName.ToLower() == templateName.ToLower()).Select(m => m.IsOpen).FirstOrDefault();
                        opencloseItem.ID = installedTemplateList.Where(m => m.TemplateName.ToLower() == templateName.ToLower()).Select(m => m.ID).FirstOrDefault();

                        list.Add(opencloseItem);
                    }
                    break;
                case "withoutinstalled":    // 未安装
                    // 在本地检测到的模板安装包，但包名不在 数据库中已安装模板记录中
                    IList<string> zipFilePaths = DetectInstallZip(httpContextBase.Server.MapPath(@"~\Upload\TemplateInstallZip"));
                    foreach (string zipFilePath in zipFilePaths)
                    {
                        FileInfo fileInfo = new FileInfo(zipFilePath);
                        string templateName = fileInfo.Name.Remove(fileInfo.Name.LastIndexOf('.'));
                        if (!installedTemplateNames.Contains(templateName, new TemplateNameComparer()))
                        {
                            InstallZipItem zipItem = new InstallZipItem(zipFilePath);
                            list.Add(zipItem);
                        }
                    }
                    break;
                default:
                    break;
            }

            this.List = list;
            this.PageInfo = new PageInfo
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalRecordCount = totalCount,
                MaxLinkCount = 10
            };
        }

        #region Helpers

        #region 检测模板安装包
        /// <summary>
        /// 检测安装包目录下存在的安装包
        /// </summary>
        /// <param name="installZipDir">安装包目录 ~/Upload/TemplateInstallZip</param>
        /// <returns>返回存在的安装包文件名（包含路径）</returns>
        private IList<string> DetectInstallZip(string installZipDir)
        {
            // 返回 安装包文件名（包括路径）
            IList<string> rtn = new List<string>();
            // 从目录检测存在的模板安装包 (.zip文件)
            string[] installZipFilePaths = Directory.GetFiles(installZipDir, "*.zip");
            foreach (string filePath in installZipFilePaths)
            {
                rtn.Add(filePath);
            }

            return rtn;
        }
        #endregion

        #endregion
    }

    public enum Source
    {
        /// <summary>
        /// 本地上传
        /// </summary>
        Upload,

        /// <summary>
        /// 应用市场
        /// </summary>
        AppStore,
    }

    public static class SourceExt
    {
        public static string GetStr(this Source source)
        {
            string rtn = string.Empty;
            switch (source)
            {
                case Source.Upload:
                    rtn = "本地上传";
                    break;
                case Source.AppStore:
                    rtn = "应用市场";
                    break;
                default:
                    break;
            }

            return rtn;
        }
    }

    public class TemplateNameComparer : IEqualityComparer<string>
    {
        public bool Equals(string x, string y)
        {
            if (x == null || y == null)
            {
                return false;
            }
            return x.ToLower() == y.ToLower();
        }

        public int GetHashCode(string obj)
        {
            throw new NotImplementedException();
        }
    }
}