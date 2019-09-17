using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CoreMVCDemo
{
    //[Route("[controller]")]
    [Route("[controller]/[action]")]//最好在控制器上只设置一次，而不是在控制器中的每个操作方法中包含[action]标记
    public class DepartmentsController : Controller
    {
        //[Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }

        //[Route("[action]")]
        //[Route("")] // 使 List()成为默认路由入口(经过实验，如果controller上加了[Route("[controller]/[action]")]，这个就不起作用了)
        public string List()
        {
            return " DepartmentsController控制器中的List()方法";
        }

        //[Route("[action]")]
        public string Details()
        {
            return " DepartmentsController控制器中的Details()方法";
        }
    }
}