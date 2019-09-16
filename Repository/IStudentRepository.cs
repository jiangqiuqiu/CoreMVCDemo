﻿using CoreMVCDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVCDemo.Repository
{
    public interface IStudentRepository
    {
        Student GetStudent(int id);
        void Save(Student student);
    }
}
