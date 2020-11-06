using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace HelpdeskDAL
{
    public class EmployeeDAO
    {
        readonly IRepository<Employees> repository;

        public EmployeeDAO()
        {
            repository = new HelpdeskRepository<Employees>();
        }


        public Employees GetByEmail(string email)
        {
            try
            {
                return repository.GetByExpression(emp => emp.Email == email).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        public Employees GetByLastName(string name)
        {
            Employees selectedEmployee = null;
            try
            {
                HelpdeskContext _db = new HelpdeskContext();
                selectedEmployee = _db.Employees.FirstOrDefault(emp => emp.LastName == name);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return selectedEmployee;
        }

        public Employees GetById(int id)
        {
            try
            {
                return repository.GetByExpression(emp => emp.Id == id).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        public List<Employees> GetAll()
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

        public int Add(Employees newEmployee)
        {
            try
            {
                newEmployee = repository.Add(newEmployee);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return newEmployee.Id;
        }

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

       /* public UpdateStatus Update(Employees employeeForUpdate)
        {
            throw new NotImplementedException();
        }*/

        /*        public int Update(Employees updatedEmployee)
                {

                    int employeeUpdated = -1;
                    try
                    {
                        HelpdeskContext _db = new HelpdeskContext();
                        Employees currentEmployee = _db.Employees.FirstOrDefault(emp => emp.Id == updatedEmployee.Id);
                        _db.Entry(currentEmployee).CurrentValues.SetValues(updatedEmployee);
                        employeeUpdated = _db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Problem in " + GetType().Name + " " +
                            MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                        throw ex;
                    }

                    return employeeUpdated;

                }*/


        public UpdateStatus Update(Employees updatedEmployee)
        {

            UpdateStatus operationStatus = UpdateStatus.Failed;
            try
            {
                operationStatus = repository.Update(updatedEmployee);
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
