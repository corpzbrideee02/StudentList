using ExercisesDAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace ExercisesViewModels
{
    public class CourseViewModel
    {
        readonly private CourseDAO _model;

        public int Id { get; set; }
        public string Name { get; set; }

        public CourseViewModel()
        {
            _model = new CourseDAO();
        }

        public void GetByName()
        {
            try
            {
                Courses cs= _model.GetByName(Name);
                Name = cs.Name;
                Id = cs.Id;
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Name = "not found";
            }
            catch (Exception ex)
            {
                Name = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        public void GetById()
        {
            try
            {
                Courses cs = _model.GetById(Id);
                Name = cs.Name;
                Id = cs.Id;
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Name = "not found";
            }
            catch (Exception ex)
            {
                Name = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

        }

        public List<CourseViewModel> GetAll(int id)
        {
            List<CourseViewModel> allVms = new List<CourseViewModel>();
            try
            {

                List<Courses> allCourses = _model.GetAll(id);

                foreach (Courses cs in allCourses)
                {
                    CourseViewModel csVm = new CourseViewModel
                    {
                        Name = cs.Name,
                        Id = cs.Id
                    };
                    allVms.Add(csVm);
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
                Courses cs= new Courses
                {
                    Name = Name
                };
                    Id = _model.Add(cs);

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
            UpdateStatus coursesUpdated = UpdateStatus.Failed;
            try
            {
                Courses cs = new Courses
                {
                    Name = Name,
                    Id= Id
                };

                coursesUpdated = _model.Update(cs);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return Convert.ToInt16(coursesUpdated);
        }


        public int Delete()
        {
            int coursesDeleted = -1;
            try
            {
                coursesDeleted = _model.Delete(Id);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return coursesDeleted;
        }



    }
}
