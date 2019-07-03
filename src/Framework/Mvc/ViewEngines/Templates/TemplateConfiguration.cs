using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Mvc.ViewEngines.Templates
{
    public class TemplateConfiguration
    {
        public TemplateConfiguration(string templateName, string path, string jsonStr)
        {
            this.TemplateName = templateName;
            this.Path = path;

            Dictionary<string, object> jsonDic = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonStr);
            if (jsonDic.ContainsKey("previewImageUrl"))
            {
                this.PreviewImageUrl = jsonDic["previewImageUrl"].ToString();
            }
            if (jsonDic.ContainsKey("description"))
            {
                this.Description = jsonDic["description"].ToString();
            }
            if (jsonDic.ContainsKey("title"))
            {
                this.Title = jsonDic["title"].ToString();
            }
            if (jsonDic.ContainsKey("authors"))
            {
                JArray authorArr = (JArray)jsonDic["authors"];
                this.Authors = new List<string>();
                foreach (var author in authorArr)
                {
                    this.Authors.Add(author.ToString());
                }
            }
        }

        /// <summary>
        /// 绝对物理路径
        /// <para>F:\Com\me\Repos\TES\WebUI\Templates\DefaultClean</para>
        /// </summary>
        public string Path { get; protected set; }

        /// <summary>
        /// ~/Templates/DefaultClean/preview.jpg
        /// </summary>
        public string PreviewImageUrl { get; protected set; }

        public string Description { get; protected set; }

        public IList<string> Authors { get; protected set; }

        public string TemplateName { get; protected set; }

        public string Title { get; protected set; }
    }
}
