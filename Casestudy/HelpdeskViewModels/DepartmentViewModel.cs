using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using HelpdeskDAL;

namespace HelpdeskViewModels
{
    public class DepartmentViewModel
    {
        readonly private DepartmentDAO _model;
        public string Name { get; set; }
        public int Id { get; set; }

        public DepartmentViewModel()
        {
            _model = new DepartmentDAO();
        }

        public List<DepartmentViewModel> GetAll()
        {
            List<DepartmentViewModel> allVms = new List<DepartmentViewModel>();
            try
            {
                List<Departments> allDepartment = _model.GetAll();

                foreach (Departments dept in allDepartment)
                {
                    DepartmentViewModel deptVm = new DepartmentViewModel();

                    deptVm.Id = dept.Id;
                    deptVm.Name = dept.DepartmentName;
                    allVms.Add(deptVm);
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
