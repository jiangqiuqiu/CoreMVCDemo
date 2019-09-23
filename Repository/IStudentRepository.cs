using CoreMVCDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVCDemo.Repository
{
    public interface IStudentRepository
    {
        /// <summary>
        /// 通过Id来获取学生信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Student GetStudent(int id);
        /// <summary>
        /// 获取所有学生信息
        /// </summary>
        /// <returns></returns>
        IEnumerable<Student> GetAllStudents();
        /// <summary>
        /// 保存学生信息
        /// </summary>
        /// <param name="student"></param>
        void Save(Student student);
        /// <summary>
        /// 添加学生信息
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        Student Add(Student student);
        /// <summary>
        /// 更新学生信息
        /// </summary>
        /// <param name="updateStudent"></param>
        /// <returns></returns>
        Student Update(Student updateStudent);
        /// <summary>
        /// 根据Id删除学生信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Student Delete(int id);
    }
}
