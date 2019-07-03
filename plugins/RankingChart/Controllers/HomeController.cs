using Core;
using Domain;
using RankingChart.Models;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RankingChart.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View("~/Plugins/RankingChart/Views/Home/Index.cshtml");
        }

        public JsonResult GetData()
        {
            EvaResultService evaResultService = Container.Instance.Resolve<EvaResultService>();
            IList<EvaResult> allEvaResult = evaResultService.GetAll();
            allEvaResult = allEvaResult.OrderBy(m => m.EvaluateTask.EndDate).ToList();
            //IList<EvaTask> allEvaTask = Container.Instance.Resolve<EvaTaskService>().GetAll().OrderBy(m => m.EndDate).ToList();
            IList<JsonDataModel> jsonArr = new List<JsonDataModel>();
            foreach (var item in allEvaResult)
            {
                bool isExistDate = false;
                bool isExistUser = false;
                if (jsonArr.Count >= 1)
                {
                    isExistDate = !jsonArr.Select(m => m.date).Contains(item.EvaluateTask.EndDate.ToString("yyyy-MM-dd"));
                    isExistUser = !jsonArr.Select(m => m.name).Contains($"{item.Teacher.Name}({item.Teacher.EmployeeCode})");
                }
                if (isExistDate && isExistUser)
                {
                    // 存在分数累加
                    var existUser = jsonArr.Where(m => m.name == $"{item.Teacher.Name}({item.Teacher.EmployeeCode})" && m.date == item.EvaluateTask.EndDate.ToString("yyyy-MM-dd")).FirstOrDefault();

                    jsonArr.Remove(existUser);
                    existUser.value = (Convert.ToDecimal(existUser.value) + item.Score).ToString();
                    jsonArr.Add(existUser);
                }
                else
                {
                    // 不存在新增
                    jsonArr.Add(new JsonDataModel
                    {
                        name = $"{item.Teacher.Name}({item.Teacher.EmployeeCode})",
                        value = item.Score.ToString("#.##"),
                        date = item.EvaluateTask.EndDate.ToString("yyyy-MM-dd")
                    });
                }
            }

            return Json(jsonArr, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserFace()
        {
            IList<EmployeeInfo> allEmployee = Container.Instance.Resolve<EmployeeInfoService>().GetAll();
            IList<UserInfo> allUser = Container.Instance.Resolve<UserInfoService>().GetAll();

            Dictionary<string, string> dic = new Dictionary<string, string>();
            foreach (var item in allEmployee)
            {
                UserInfo userInfo = allUser.Where(m => m.ID == item.UID).FirstOrDefault();
                dic.Add($"{item.Name}({item.EmployeeCode})", userInfo.Avatar);
            }

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserColor()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            IList<EmployeeInfo> allEmployee = Container.Instance.Resolve<EmployeeInfoService>().GetAll();
            foreach (var item in allEmployee)
            {
                dic.Add($"{item.Name}({item.EmployeeCode})", SingleColor(item.ID.ToString()));
            }

            return Json(dic, JsonRequestBehavior.AllowGet);
        }

        private string SingleColor(string id)
        {
            string color16 = string.Empty;
            int r = 0, g = 0, b = 0;
            r = int.Parse(id) % 255;
            g = (int.Parse(id) + 54) % 255;
            b = (int.Parse(id) + 156) % 255;
            color16 = System.Drawing.ColorTranslator.ToHtml(System.Drawing.Color.FromArgb(r, g, b));

            return color16;
        }
    }
}