using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using HelpdeskDAL;

namespace HelpdeskViewModels
{
    public class ProblemViewModel
    {
        readonly private ProblemDAO _model;
        public string Description { get; set; }
        public int Id { get; set; }
        public string Timer { get; set; }

        public ProblemViewModel()
        {
            _model = new ProblemDAO();
        }

        public List<ProblemViewModel> GetAll()
        {
            List<ProblemViewModel> allVms = new List<ProblemViewModel>();
            try
            {
                List<Problems> allProblems = _model.GetAll();

                foreach (Problems prob in allProblems)
                {
                    ProblemViewModel probVm = new ProblemViewModel();

                    probVm.Id = prob.Id;
                    probVm.Description = prob.Description;
                    allVms.Add(probVm);
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

        public void GetByDescription()
        {
            try
            {
                Problems desc = _model.GetByDescription(Description);
                Description = desc.Description;
                Id = desc.Id;
                Timer = Convert.ToBase64String(desc.Timer);
            }
            catch (NullReferenceException nex)
            {
                Debug.WriteLine(nex.Message);
                Description = "not found";
            }
            catch (Exception ex)
            {
                Description = "not found";
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }



    }
}
