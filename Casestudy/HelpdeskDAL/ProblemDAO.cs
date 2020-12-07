using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

namespace HelpdeskDAL
{
    public class ProblemDAO
    {

        readonly IRepository<Problems> repository;

        public ProblemDAO()
        {
            repository = new HelpdeskRepository<Problems>();
        }
        public List<Problems> GetAll()
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

        public Problems GetByDescription(string desc)
        {
            Problems selectedProblem = null;
            try
            {
                HelpdeskContext _db = new HelpdeskContext();
                selectedProblem = _db.Problems.FirstOrDefault(prob => prob.Description==desc);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return selectedProblem;
        }

    }
}
