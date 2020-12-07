
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ExercisesDAL
{
    public class CourseDAO
    {
        readonly IRepository<Courses> repository;

        readonly IRepository<Grades> repositoryG;

        readonly IRepository<Students> repositoryS;

        public CourseDAO()
        {
            repository = new SomeSchoolRepository<Courses>();
            repositoryG = new SomeSchoolRepository<Grades>();
         //   repositoryS = new SomeSchoolRepository<Students>();
        }


        public Courses GetById(int id)
        {
            try
            {
                return repository.GetByExpression(stu => stu.Id == id).FirstOrDefault();

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
        public Courses GetByName(string name)
        {
            try
            {
                return repository.GetByExpression(crs => crs.Name == name).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
        public List<Courses> GetAll(int id)
        {
            List<Courses> studentCourse = new List<Courses>();

           List<Grades> studentGrades = repositoryG.GetAll();

            try
            {
                foreach (Grades g in studentGrades)
                {
                    if(g.StudentId==id)
                    {

                        Courses course = g.Course;
                        studentCourse.Add(course);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }


            return studentCourse;
        }

        public int Add(Courses newCourses)
        {
            try
            {
                newCourses = repository.Add(newCourses);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return newCourses.Id;

        }



        public int Delete(int id)
        {
            try
            {
                return repository.Delete(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }


        public UpdateStatus Update(Courses updatedCourse)
        {

            UpdateStatus operationStatus = UpdateStatus.Failed;
            try
            {
                operationStatus = repository.Update(updatedCourse);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                operationStatus = UpdateStatus.Stale;
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                   MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);

            }
            return operationStatus;
        }


    }
}
