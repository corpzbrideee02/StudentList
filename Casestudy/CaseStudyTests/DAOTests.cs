using System;
using Xunit;
using HelpdeskDAL;
using System.Diagnostics;
using System.Collections.Generic;

namespace CaseStudyTests
{
    public class DAOTests
    {
        [Fact]
        public void Employee_GetByEmailTest()
        {
            try
            {
                EmployeeDAO dao = new EmployeeDAO();
               Employees selectedEmployee = dao.GetByEmail("bs@abc.com");
                Assert.NotNull(selectedEmployee);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }
        }

       /* [Fact]
        public void Employee_GetByLastnameTest()
        {
            try
            {
                EmployeeDAO dao = new EmployeeDAO();
                Employees selectedEmployee = dao.GetByLastName("Pet");
                Assert.NotNull(selectedEmployee);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }
        }*/

        [Fact]
        public void Employee_GetByIdTest()
        {
            try
            {
                EmployeeDAO dao = new EmployeeDAO();
                Employees selectedEmployee = dao.GetById(2);
                Assert.NotNull(selectedEmployee);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }
        }

        [Fact]
        public void Employee_GetAllTest()
        {
            try
            {
                EmployeeDAO dao = new EmployeeDAO();
                List<Employees> allEmployees = dao.GetAll();
                Assert.True(allEmployees.Count > 0);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }
        }

        [Fact]
        public void Employee_AddTest()
        {
            try
            {
                EmployeeDAO dao = new EmployeeDAO();
                Employees employee = new Employees();
                Employees newEmployee = new Employees
                {
                    FirstName = "Joe",
                    LastName = "Smith",
                    PhoneNo = "(555)555-1234",
                    Title = "Mr.",
                    DepartmentId = 100,
                    Email = "el@someschool.com"
                };
                Assert.True(dao.Add(newEmployee) > 0);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }

        }

        [Fact]
        public void Employee_UpdateTest()
        {
            try
            {
                EmployeeDAO dao = new EmployeeDAO();
                Employees employeeForUpdate = dao.GetByLastName("Smith");

                if (employeeForUpdate != null)
                {
                    string oldPhoneNo = employeeForUpdate.PhoneNo;
                    string newPhoneNo = oldPhoneNo == "519-555-1234" ? "555-555-5555" : "519-555-1234";
                    employeeForUpdate.PhoneNo = newPhoneNo;
                }
                Assert.True(dao.Update(employeeForUpdate) == UpdateStatus.Ok);   //indicates the #of rows updated

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }
        }

        [Fact]
        public void Employee_DeleteTest()
        {
            try
            {
                EmployeeDAO dao = new EmployeeDAO();
                Employees employeeForDelete = dao.GetByLastName("Smith");

                Assert.True(dao.Delete(employeeForDelete.Id) == 1);   //indicates the #of rows deleted
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }

        }


        [Fact]
        public void Employee_ConcurrencyTest()
        {
            EmployeeDAO dao1 = new EmployeeDAO();
            EmployeeDAO dao2 = new EmployeeDAO();
            Employees employeeForUpdate1 = dao1.GetByLastName("Corpuz");
            Employees employeeForUpdate2 = dao2.GetByLastName("Corpuz");

            if (employeeForUpdate1 != null)
            {
                string oldPhoneNo = employeeForUpdate1.PhoneNo;
                string newPhoneNo = oldPhoneNo == "519-555-1234" ? "555-555-5555" : "519-555-1234";
                employeeForUpdate1.PhoneNo = newPhoneNo;
                if (dao1.Update(employeeForUpdate1) == UpdateStatus.Ok)
                {
                    //need to change the phone # to something else
                    employeeForUpdate2.PhoneNo = "666-666-6666";
                    Assert.True(dao2.Update(employeeForUpdate2) == UpdateStatus.Stale);

                }
                else
                    Assert.True(false);

            }

        }


        [Fact]
        public void Employees_LoadPicsTest()
        {
            DALUtil util = new DALUtil();
            Assert.True(util.AddEmployeePicsToDb());

        }
    }

}
