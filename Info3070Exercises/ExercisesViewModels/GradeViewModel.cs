using ExercisesDAL;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace ExercisesViewModels
{
    public class GradeViewModel
    {
        readonly private GradeDAO _model;


        public int Id { get; set; }
        public int StudentId { get; set; }
        public int CourseId { get; set; }
        public int Mark { get; set; }
        public string Comments { get; set; }
        public string Timer { get; set; }

        public GradeViewModel()
        {
            _model = new GradeDAO();
        }


        public void GetById()
        {
            try
            {
                Grades gd = _model.GetById(Id);
                StudentId = gd.StudentId;
                CourseId = gd.CourseId;
                Mark = gd.Mark;
                Comments = gd.Comments;
                Id = gd.Id;
                Timer = Convert.ToBase64String(gd.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

        }

        public List<GradeViewModel> GetAll(int id)
        {
            List<GradeViewModel> allVms = new List<GradeViewModel>();
            try
            {

                List<Grades> allGrades = _model.GetAll(id);

                foreach (Grades gd in allGrades)
                {
                    GradeViewModel gdVm = new GradeViewModel();
                    
                    gdVm.StudentId = gd.StudentId;
                    gdVm.CourseId = gd.CourseId;
                    gdVm.Mark = gd.Mark;
                    gdVm.Comments = gd.Comments;
                    gdVm.Id = gd.Id;
                    gdVm.Timer = Convert.ToBase64String(gd.Timer);
                    allVms.Add(gdVm);
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
                Grades gd = new Grades
                {
                    StudentId=StudentId,
                    CourseId=CourseId,
                    Mark=Mark,
                    Comments=Comments
                 };
                Id = _model.Add(gd);

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
            UpdateStatus gradesUpdated = UpdateStatus.Failed;
            try
            {
                Grades gd = new Grades
                {
                    StudentId = StudentId,
                    CourseId = CourseId,
                    Mark = Mark,
                    Id = Id,
                    Comments = Comments
            };

               gd.Timer = Convert.FromBase64String(Timer);
                gradesUpdated = _model.Update(gd);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return Convert.ToInt16(gradesUpdated);
        }


        public int Delete()
        {
            int gradesDeleted = -1;
            try
            {
                gradesDeleted = _model.Delete(Id);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return gradesDeleted;
        }



    }
}
