using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebUI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CourseBoxApiController : ApiController
    {
    }
}
