using ExercisesDAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace ExercisesViewModels
{
    public class StudentViewModel
    {
        readonly private StudentDAO _model;
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email{ get; set; }
        public string Phoneno { get; set; }
        public string Timer{ get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int Id { get; set; }
        public string Picture64 { get; set; }

        public StudentViewModel()
        {
            _model = new StudentDAO();
        }
        //
        //find student using Lastname property
        //

        public void GetByLastname()
        {
            try
            {
                Students stu = _model.GetByLastName(Lastname);
                Title = stu.Title;
                Firstname = stu.FirstName;
                Lastname = stu.LastName;
                Phoneno = stu.PhoneNo;
                Email = stu.Email;
                Id = stu.Id;
                DivisionId = stu.DivisionId;
                if(stu.Picture!=null)
                {
                    Picture64 = Convert.ToBase64String(stu.Picture);
                }
                Timer = Convert.ToBase64String(stu.Timer);
            }
            catch(NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Lastname = "not found";
            }
            catch(Exception ex)
            {
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        public void GetById()
        {
            try
            {
                Students stu = _model.GetById(Id);
                Title = stu.Title;
                Firstname = stu.FirstName;
                Lastname = stu.LastName;
                Phoneno = stu.PhoneNo;
                Email = stu.Email;
                Id = stu.Id;
                DivisionId = stu.DivisionId;
                if (stu.Picture != null)
                {
                    Picture64 = Convert.ToBase64String(stu.Picture);
                }
                Timer = Convert.ToBase64String(stu.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Lastname = "not found";
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

        }

        public List<StudentViewModel> GetAll()
        {
            List<StudentViewModel> allVms = new List<StudentViewModel>();
            try
            {

                List<Students> allStudents = _model.GetAll();

                foreach(Students stu in allStudents)
                {
                    StudentViewModel stuVm = new StudentViewModel();
                    stuVm.Title = stu.Title;
                    stuVm.Firstname = stu.FirstName;
                    stuVm.Lastname = stu.LastName;
                    stuVm.Phoneno = stu.PhoneNo;
                    stuVm.Email = stu.Email;
                    stuVm.Id = stu.Id;
                    stuVm.DivisionId = stu.DivisionId;
                    stuVm.DivisionName = stu.Division.Name;
                    stuVm.Timer = Convert.ToBase64String(stu.Timer);
                    allVms.Add(stuVm);
                }
                
            }
          
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return allVms;
        }

        public void Add()
        {
            Id = -1;
            try
            {
                Students stu = new Students
                {

                Title = Title,
                FirstName =Firstname,
                LastName = Lastname,
                PhoneNo = Phoneno,
                Email = Email,
                DivisionId = DivisionId
            };
                Id = _model.Add(stu);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        public int Update()
         {
            UpdateStatus studentsUpdated = UpdateStatus.Failed;
            try
             {
                 Students stu = new Students
                 {
                     Title = Title,
                     FirstName = Firstname,
                     LastName = Lastname,
                     PhoneNo = Phoneno,
                     Email = Email,
                     Id=Id,
                     DivisionId = DivisionId
                 };

                 if (Picture64 != null)
                 {
                     stu.Picture = Convert.FromBase64String(Picture64);
                 }
                 stu.Timer = Convert.FromBase64String(Timer);
                 studentsUpdated = _model.Update(stu);

             }
             catch (Exception ex)
             {
                 Debug.WriteLine("Problem in " + GetType().Name + " " +
                     MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                 throw ex;
             }
            return Convert.ToInt16(studentsUpdated);
         }


       /* public UpdateStatus Update()
        {
            //int studentsUpdated = -1;

            UpdateStatus studentsUpdated = UpdateStatus.Failed;
            try
            {
                Students stu = new Students
                {
                    Title = Title,
                    FirstName = Firstname,
                    LastName = Lastname,
                    PhoneNo = Phoneno,
                    Email = Email,
                    Id = Id,
                    DivisionId = DivisionId
                };

                if (Picture64 != null)
                {
                    stu.Picture = Convert.FromBase64String(Picture64);
                }
                stu.Timer = Convert.FromBase64String(Timer);
                //studentsUpdated = _dao.Update(stu);
                studentsUpdated = _dao.Update(stu);
                studentsUpdated = UpdateStatus.Ok;


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return studentsUpdated;
        }*/

         public int Delete()
          {
             int studentsDeleted = -1;
             try
             {
                 studentsDeleted = _model.Delete(Id);

             }
             catch (Exception ex)
             {
                 Debug.WriteLine("Problem in " + GetType().Name + " " +
                     MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                 throw ex;
             }
             return studentsDeleted;
          }


               
    }
}
