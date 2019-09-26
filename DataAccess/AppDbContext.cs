﻿using CoreMVCDemo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVCDemo.DataAccess
{
    public class AppDbContext:DbContext
    {
        //DbContextOptions 实例承载配置信息，如连接字符串，数据库提供商使用等
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }        
        public DbSet<Student> Students { get; set; }

        //为我们的数据库添加初始数据，就称之为种子数据
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Seed();//通过扩展方法封装种子数据
        }
    }
}