
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ExercisesDAL
{
    public class GradeDAO
    {
        readonly IRepository<Grades> repository;


        public GradeDAO()
        {
            repository = new SomeSchoolRepository<Grades>();
        }


        public Grades GetById(int id)
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

        public List<Grades> GetAll(int id)
        {

            List<Grades> studentGradesFinal = new List<Grades>();
            List<Grades> studentGrades = repository.GetAll();
          // Students stud = repositoryS.GetByExpression(stu => stu.Id == id).FirstOrDefault();
          // Courses crs = repositoryC.GetByExpression(crs => crs.Id == csrId).FirstOrDefault();
            try
            {
                foreach (Grades g in studentGrades)
                {
                    //if (g.StudentId == stuId && g.CourseId==csrId)
                     if (g.StudentId == id)

                        {
                            studentGradesFinal.Add(g);
                            
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return studentGradesFinal;
        }

        public int Add(Grades newGrades)
        {
            try
            {
                newGrades = repository.Add(newGrades);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return newGrades.Id;

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


        public UpdateStatus Update(Grades updatedGrades)
        {

            UpdateStatus operationStatus = UpdateStatus.Failed;
            try
            {
                operationStatus = repository.Update(updatedGrades);
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
