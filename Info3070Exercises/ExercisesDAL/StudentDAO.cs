using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ExercisesDAL
{
    public class StudentDAO
    {
        readonly IRepository<Students> repository;

        public StudentDAO()
        {
            repository = new SomeSchoolRepository<Students>();
        }

        public Students GetByLastName(string name)
        {
            try
            {
                return repository.GetByExpression(stu => stu.LastName == name).FirstOrDefault();
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        public Students GetById(int id)
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

        public List<Students> GetAll()
        {
          
            try
            {
                return repository.GetAll();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        public int Add(Students newStudent)
        {
            try
            {
                newStudent = repository.Add(newStudent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return newStudent.Id;

        }

        /*public int Delete(int id)
        {
            int studentsDeleted = -1;
            try
            {
                SomeSchoolContext _db = new SomeSchoolContext();
                Students selectedStudent = _db.Students.FirstOrDefault(stu => stu.Id ==id);
                _db.Students.Remove(selectedStudent);
                studentsDeleted = _db.SaveChanges();    //returns # of rows removed

                return  repository.Delete(id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return studentsDeleted;
        }*/

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


        /*public UpdateStatus Update(Students updatedStudent)
        {
            UpdateStatus studentUpdated = UpdateStatus.Failed;
            try
            {
                SomeSchoolContext _db = new SomeSchoolContext();
                Students currentStudent = _db.Students.FirstOrDefault(stu => stu.Id == updatedStudent.Id);
                _db.Entry(currentStudent).OriginalValues["Timer"] = updatedStudent.Timer;
                _db.Entry(currentStudent).CurrentValues.SetValues(updatedStudent);
               if(_db.SaveChanges()==1)
                {
                    studentUpdated = UpdateStatus.Ok;
                }
            }
            catch(DbUpdateConcurrencyException ex)
            {
                studentUpdated = UpdateStatus.Stale;
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                   MethodBase.GetCurrentMethod().Name + " " + ex.Message);
            }

            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
              
            }

            return studentUpdated;

        }*/

        public UpdateStatus Update(Students updatedStudent)
        {

            UpdateStatus operationStatus = UpdateStatus.Failed;
            try
            {
                operationStatus= repository.Update(updatedStudent);
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
