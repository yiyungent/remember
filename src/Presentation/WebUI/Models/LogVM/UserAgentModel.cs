using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebUI.Models.LogVM
{
    public class UserAgentModel
    {
        public string UA { get; set; }

        public BrowserModel Browser { get; set; }

        public EngineModel Engine { get; set; }

        public OsModel OS { get; set; }

        public DeviceModel Device { get; set; }

        public CpuModel Cpu { get; set; }

        public sealed class BrowserModel
        {
            public string Name { get; set; }
            public string Version { get; set; }
        }

        public sealed class EngineModel
        {
            public string Name { get; set; }
            public string Version { get; set; }
        }

        public sealed class OsModel
        {
            public string Name { get; set; }
            public string Version { get; set; }
        }

        public sealed class DeviceModel
        {
            public string Model { get; set; }
            public string Type { get; set; }
            public string Vendor { get; set; }
        }

        public sealed class CpuModel
        {
            public string Architecture { get; set; }
        }
    }
}