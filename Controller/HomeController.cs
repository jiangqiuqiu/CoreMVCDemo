﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMVCDemo.Models;
using CoreMVCDemo.Repository;
using CoreMVCDemo.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CoreMVCDemo
{
    public class HomeController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        //使用构造函数注入的方式注入IStudentRepository
        public HomeController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public string Index()
        {
            return _studentRepository.GetStudent(1).Name;
        }

        public ViewResult Details()
        {
            Student model = _studentRepository.GetStudent(1);
            //return View(model);

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
                Student = _studentRepository.GetStudent(1),
                PageTitle = "Student Details"
            };

            // 将ViewModel对象传递给View()方法
            return View(homeDetailsViewModel);
        }
    }
}