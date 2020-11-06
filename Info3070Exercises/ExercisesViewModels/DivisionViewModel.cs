using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using ExercisesDAL;

namespace ExercisesViewModels
{
    public class DivisionViewModel
    {
        readonly private DivisionDAO _model;
        public string Name { get; set; }
        public int Id { get; set; }

        public DivisionViewModel()
        {
            _model = new DivisionDAO();
        }

        public List<DivisionViewModel> GetAll()
        {
            List<DivisionViewModel> allVms = new List<DivisionViewModel>();
            try
            {
                List<Divisions> allDivisions = _model.GetAll();

                foreach (Divisions div in allDivisions)
                {
                    DivisionViewModel divVm = new DivisionViewModel();
                    
                        divVm.Id= div.Id;
                        divVm.Name = div.Name;
                        allVms.Add(divVm);
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


    }
}
