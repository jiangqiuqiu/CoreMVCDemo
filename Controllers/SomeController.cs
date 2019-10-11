using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreMVCDemo.Controllers
{
    [Authorize(Roles = "admin,user")]
    public class SomeController : Controller
    {
        public string ABC()
        {
            return "ABC操作方法";
        }

        [Authorize(Roles = "admin")]
        public string XYZ()
        {
            return "XYZ操作方法";
        }

        [AllowAnonymous]
        public string Anyone()
        {
            return "Anyone操作方法";
        }
    }
}