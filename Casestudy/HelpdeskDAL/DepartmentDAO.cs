using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace HelpdeskDAL
{
    public class DepartmentDAO
    {

        readonly IRepository<Departments> repository;

        public DepartmentDAO()
        {
            repository = new HelpdeskRepository<Departments>();
        }
        public List<Departments> GetAll()
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
    }
}
