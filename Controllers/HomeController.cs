﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreMVCDemo.Models;
using CoreMVCDemo.Repository;
using CoreMVCDemo.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreMVCDemo.Controllers
{
    //将应用于控制器操作方法的路由模板移动到了控制器上,精简代码
    //[Route("Home")]

    //表示当前控制器需要有用户认证
    //实际情况就是，当访问该控制器下的方法时候就需要登录
    //[Authorize]
    [AllowAnonymous]//本控制器允许匿名访问
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly HostingEnvironment environmentHosting;//不加下划线是因为这个是系统层级的，而不是自己定义的仓储
        private readonly ILogger<HomeController> logger;

        //使用构造函数注入的方式注入IStudentRepository
        public HomeController(IStudentRepository studentRepository,HostingEnvironment environmentHosting,ILogger<HomeController> logger)
        {
            _studentRepository = studentRepository;
            this.environmentHosting = environmentHosting;
            this.logger = logger;
        }

        //需要记住的一个非常重要的一点是, 如果操作方法上的路由模板以/或 ~/开头, 
        //则控制器路由模板不会与操作方法路由模板组合在一起。
        //[Route("/")]//不加这个 http://localhost:10153/ 就访问不到了
        //[Route("")]
        //[Route("Home")]
        //[Route("Home/Index")]
        //[Route("Index")]
        public ViewResult Index()
        {
            //返回id是1的学生姓名，返回值是string
            //return _studentRepository.GetStudent(1).Name;


            //查询所有的学生信息
            var model = _studentRepository.GetAllStudents();
            //将学生列表传递到视图
            return View(model);
        }

        //?使路由模板中的id参数为可选，如果要使它为必选，删除?即可
        //[Route("Home/Details/{id?}")]
        //[Route("Details/{id?}")]
        //? 使id方法参数可以为空
        //public ViewResult Details(int? id)
        //[Authorize]//也可以放在具体Action上
        //[AllowAnonymous]//允许匿名访问
        public ViewResult Details(int id)
        {
            logger.LogTrace("Trace(跟踪) Log");
            logger.LogDebug("Debug(调试) Log");
            logger.LogInformation("Information(信息)Log");
            logger.LogWarning("Warning(警告)Log");
            logger.LogError("Error(错误)Log");
            logger.LogCritical("Critical(严重)Log");



            //用于测试异常捕捉的
            //throw new Exception("此异常发生在Details视图中");


            //?? 如果"id"为null，则使用1，否则使用路由中传递过来的值
            //Student model = _studentRepository.GetStudent(id??1);
            //return View(model);

            //404处理
            Student student = _studentRepository.GetStudent(id);
            if (student==null)
            {
                Response.StatusCode = 404;
                return View("StudentNotFound",id);
            }

            //View（string viewName） 方法
            //MVC 查找名为 **“Test.cshtml”而不是“Details.cshtml”**的视图文件。
            //如果我们没有指定视图名称，它会查找 “Details.cshtml”
            //return View("Test");

            //指定视图文件路径
            //请注意：使用绝对路径，会项目的根目录开始搜索，我们可以使用/或〜/
            //return View("Views/Home/Test.cshtml");
            //return View("/Views/Home/Test.cshtml");
            //return View("~/Views/Home/Test.cshtml");


            //相对视图文件路径
            //使用相对路径，我们不指定文件扩展名 .cshtml
            //如果你要的返回值在文件夹层次结构中超过了 2 个深度，请使用../两次
            //return View("../Home/Test");

            //其他重载方法：
            /*********************
             重载方法	                               描述
            View(object model)	            使用此重载方法将模型数据从控制器传递到视图
       View(string viewName, object model)	传递视图名称和模型数据             
             */


            //将数据从控制器传递到视图的三种方法

            //1、使用 ViewData（弱类型） 将数据从 Controller 传递到视图
            // 使用ViewData将PageTitle和Student模型传递给View
            //ViewData["PageTitle"] = "Student Details";
            //ViewData["Student"] = model;
            //return View();

            //2、使用ViewBag将数据从控制器传递到视图
            //实际上，ViewBag是ViewData的包装器。 
            //使用ViewData，我们使用 string 类型的键名来存储和查询数据。 
            //而使用ViewBag，我们则使用的是动态属性而不是字符串键。
            // 将PageTitle和Student模型对象存储在ViewBag
            // 我们正在使用动态属性PageTitle和Student
            //ViewBag.PageTitle = "Student Details";
            //ViewBag.Student = model;

            //return View();


            //3、强类型视图 - 控制器代码
            //将数据从控制器传递到视图的首选方法是使用强类型视图
            //ViewBag.PageTitle = "Student Details";

            //return View(model);


            //
            //使用DTO
            //
            //实例化HomeDetailsViewModel并存储Student详细信息和PageTitle
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                //Student = _studentRepository.GetStudent(id??1),
                Student = student,
                PageTitle = "学生详细信息"
            };

            // 将ViewModel对象传递给View()方法
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(StudentCreateViewModel model)
        {
            if (ModelState.IsValid)//使用ModelState.IsValid 属性会检查验证是否失败或成功
            {
                //添加一个学生信息
                //Student newStudent = _studentRepository.Add(student);
                //return RedirectToAction("Details", new { id = newStudent.Id });

                //单文件（图片）上传+添加学生信息
                //string uniqueFileName = null;
                //if (model.Photo != null)
                //{
                //    string uploadsFolder = Path.Combine(environmentHosting.WebRootPath, "images");
                //    uniqueFileName = $"{Guid.NewGuid().ToString()}_{model.Photo.FileName}";
                //    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //    using (var fs = new FileStream(filePath, FileMode.Create))
                //    {
                //        model.Photo.CopyTo(fs);
                //    }
                //}

                Student newStudent = new Student()
                {
                    Name = model.Name,
                    ClassName = model.ClassName,
                    Email = model.Email,
                    //PhotoPath = uniqueFileName
                    PhotoPath=ProcessUploadFile(model)
                };

                _studentRepository.Add(newStudent);
                return RedirectToAction("Details", new { id = newStudent.Id });

                //多文件（图片）上传+添加学生信息
                //string uniqueFileName = null;
                //if (model.Photos != null && model.Photos.Count>0)
                //{
                //    //必须将图像上传到wwwroot中的images文件夹
                //    //而要获取wwwroot文件夹的路径，我们需要注入ASP.NET Core提供的HostingEnviroment服务
                //    string uploadsFolder = Path.Combine(environmentHosting.WebRootPath, "images");
                //    foreach (var photo in model.Photos)
                //    {
                //        uniqueFileName = $"{Guid.NewGuid().ToString()}_{photo.FileName}";
                //        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                //        using (var fs = new FileStream(filePath, FileMode.Create))
                //        {
                //           photo.CopyTo(fs);
                //        }
                //    }                    
                //}

                //Student newStudent = new Student()
                //{
                //    Name = model.Name,
                //    ClassName = model.ClassName,
                //    Email = model.Email,
                //    PhotoPath = uniqueFileName
                //};

                //_studentRepository.Add(newStudent);
                //return RedirectToAction("Details", new { id = newStudent.Id });
            }
            return View();

            //Student newStudent = _studentRepository.Add(student);
            //return View();
        }

        //1、视图
        //2、视图模型
        //对应的页面调整
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Student student = _studentRepository.GetStudent(id);

            if (student!=null)
            {
                StudentEditViewModel studentEditViewModel = new StudentEditViewModel()
                {
                    Id = student.Id,
                    Name = student.Name,
                    Email = student.Email,
                    ClassName = student.ClassName,
                    ExistingPhotoPath = student.PhotoPath
                };
                return View(studentEditViewModel);
            }
            return View();
            //throw new Exception("查询不到学生信息");
        }

        [HttpPost]
        public IActionResult Edit(StudentEditViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                Student student = _studentRepository.GetStudent(model.Id);

                if (student!=null)
                {
                    student.Name = model.Name;
                    student.Email = model.Email;
                    student.ClassName = model.ClassName;

                    if (model.Photo!=null)
                    {
                        if (model.ExistingPhotoPath!=null)
                        {
                            string filePathDelete = Path.Combine(environmentHosting.WebRootPath,"images",model.ExistingPhotoPath);
                            System.IO.File.Delete(filePathDelete);//删除已存在的图片
                        }

                        //string uniqueFileName = null;
                        //string uploadsFolder = Path.Combine(environmentHosting.WebRootPath, "images");
                        //uniqueFileName = $"{Guid.NewGuid().ToString()}_{model.Photo.FileName}";
                        //string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        //using (var fs = new FileStream(filePath, FileMode.Create))
                        //{
                        //    model.Photo.CopyTo(fs);
                        //}

                        student.PhotoPath = ProcessUploadFile(model);
                    }
                    Student updateStudent = _studentRepository.Update(student);//更新学生信息
                    return RedirectToAction("Index");
                }
            }
            return View(model);
        }

        /// <summary>
        /// 将照片保存到指定的路径中并且返回唯一的文件名
        /// </summary>
        /// <returns></returns>
        private string ProcessUploadFile(StudentCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo!=null)
            {
                string uploadsFolder = Path.Combine(environmentHosting.WebRootPath, "images");
                uniqueFileName = $"{Guid.NewGuid().ToString()}_{model.Photo.FileName}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fs = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fs);
                }
            }            
            return uniqueFileName;
        }
    }
}