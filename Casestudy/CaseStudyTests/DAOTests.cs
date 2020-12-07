using System;
using Xunit;
using HelpdeskDAL;
using System.Diagnostics;
using System.Collections.Generic;
using Xunit.Abstractions;

namespace CaseStudyTests
{
    public class DAOTests
    {

        private readonly ITestOutputHelper output;

        public DAOTests(ITestOutputHelper output)
        {
            this.output = output;
        }
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

        [Fact]
        public void Call_ComprehensiveTest()
        {
            CallDAO cdao = new CallDAO();
            EmployeeDAO edao = new EmployeeDAO();
            ProblemDAO pdao = new ProblemDAO();
            Calls call = new Calls
            {
                DateOpened = DateTime.Now,
                DateClosed = null,
                OpenStatus=true,
                EmployeeId=edao.GetByLastName("Corpuz").Id,
                TechId=edao.GetByLastName("Burner").Id,
                ProblemId=pdao.GetByDescription("Hard Drive Failure").Id,
                Notes="Corpuz's drive is shot, Burner to fix it"

            };

            int newCallId = cdao.Add(call);
            output.WriteLine("New Call Generated    -   Id  = " + newCallId);
            call = cdao.GetById(newCallId);
            byte[] oldtimer = call.Timer;
            output.WriteLine("New Call Retrieved");
            call.Notes += "\n Ordered new drive!";

            if(cdao.Update(call)==UpdateStatus.Ok)
            {
                output.WriteLine("Call was updated " + call.Notes);
            }
            else
            {
                output.WriteLine("Call was not updated ");
            }

            call.Timer = oldtimer;
            call.Notes = "doesn't matter data is stale now";

            if(cdao.Update(call)==UpdateStatus.Stale)
            {
                output.WriteLine("Call was not updated due to stale data");
            }
            cdao = new CallDAO();
            call = cdao.GetById(newCallId);
            if(cdao.Delete(newCallId)==1)
            {
                output.WriteLine("Call was deleted");

            }
            else
            {
                output.WriteLine("Call was not deleted");
            }

            Assert.Null(cdao.GetById(newCallId));
        }







    }

}
