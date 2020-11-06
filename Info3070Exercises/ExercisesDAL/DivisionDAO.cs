using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ExercisesDAL
{
    public class DivisionDAO
    {
        readonly IRepository<Divisions> repository;

        public DivisionDAO()
        {
            repository = new SomeSchoolRepository<Divisions>();
        }
        public List<Divisions> GetAll()
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
