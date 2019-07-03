using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Areas.Admin.Models.DashboardVM
{
    public class EvaAnalyzeChart
    {
        public Title title { get; set; }
        public Tooltip tooltip { get; set; }
        public Legend legend { get; set; }
        public Toolbox toolbox { get; set; }
        public Grid grid { get; set; }
        public Xaxi[] xAxis { get; set; }
        public Yaxi[] yAxis { get; set; }
        public Series[] series { get; set; }
    }

    public class Title
    {
        public string text { get; set; }
    }

    public class Tooltip
    {
        public string trigger { get; set; }
        public Axispointer axisPointer { get; set; }
    }

    public class Axispointer
    {
        public string type { get; set; }
        public Label label { get; set; }
    }

    public class Label
    {
        public string backgroundColor { get; set; }
    }

    public class Legend
    {
        public string[] data { get; set; }
    }

    public class Toolbox
    {
        public Feature feature { get; set; }
    }

    public class Feature
    {
        public Saveasimage saveAsImage { get; set; }
    }

    public class Saveasimage
    {
    }

    public class Grid
    {
        public string left { get; set; }
        public string right { get; set; }
        public string bottom { get; set; }
        public bool containLabel { get; set; }
    }

    public class Xaxi
    {
        public string type { get; set; }
        public bool boundaryGap { get; set; }
        public string[] data { get; set; }
    }

    public class Yaxi
    {
        public string type { get; set; }
    }

    public class Series
    {
        public string name { get; set; }
        public string type { get; set; }
        public string stack { get; set; }
        public Areastyle areaStyle { get; set; }
        public int[] data { get; set; }
        public Label1 label { get; set; }
    }

    public class Areastyle
    {
        public Normal normal { get; set; }
    }

    public class Normal
    {
    }

    public class Label1
    {
        public Normal1 normal { get; set; }
    }

    public class Normal1
    {
        public bool show { get; set; }
        public string position { get; set; }
    }
}