using System;
using System.Collections.Generic;
using System.Text;
using HelpdeskViewModels;
using Xunit;
using Xunit.Abstractions;

namespace CaseStudyTests
{
    public class ViewModelTests
    {
        private readonly ITestOutputHelper output;

        public ViewModelTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void Employee_GetByLastNameTest()
        {
            EmployeeViewModel vm = new EmployeeViewModel { Lastname = "Joe" };
            vm.GetByLastname();
            Assert.NotNull(vm.Firstname);
        }
        [Fact]
        public void Employee_GetByEmail()
        {
            EmployeeViewModel vm = new EmployeeViewModel { Email= "bs@abc.com" };
            vm.GetByEmail();
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public void Employee_GetByIdTest()
        {
            EmployeeViewModel vm = new EmployeeViewModel { Lastname = "Joe" };
            vm.GetByLastname();
            vm.GetById();
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public void Employee_GetAllTest()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            List<EmployeeViewModel> allEmployeeVms = vm.GetAll();
            Assert.True(allEmployeeVms.Count > 0);
        }

        [Fact]
        public void Employee_AddTest()
        {
            EmployeeViewModel vm = new EmployeeViewModel()
            {
                Title = "Ms.",
                Firstname = "Dianne",
                Lastname = "Corpuz",
                Email = "dc@abc.com",
                Phoneno = "(555)555-5551",
                DepartmentId = 100
            };
            vm.Add();
            Assert.True(vm.Id > 0);
        }

        [Fact]
        public void Employee_UpdateTest()
        {
            EmployeeViewModel vm = new EmployeeViewModel { Email = "dc@abc.ca" };
            vm.GetByEmail();//Student just added
            vm.Email = vm.Email == "dc@abc.com" ? "dianne_corp@abc.com" : "dc@abc.com";
            int EmployeeUpdated = vm.Update();
            Assert.True(EmployeeUpdated > 0);
        }

        [Fact]
        public void Employee_DeleteTest()
        {
            EmployeeViewModel vm = new EmployeeViewModel { Email = "dc@abc.com" };
            vm.GetByEmail();//Student just added
            int EmployeeDeleted = vm.Delete();
            Assert.True(EmployeeDeleted == 1);
        }

        [Fact]
        public void Employee_ConcurrencyTest()
        {
            EmployeeViewModel vm1 = new EmployeeViewModel();
            EmployeeViewModel vm2 = new EmployeeViewModel();
            vm1.Email = "dianne_corp@abc.com";
            vm2.Email = "dianne_corp@abc.com";

            vm1.GetByEmail();
            vm2.GetByEmail();
            vm1.Email = (vm1.Email.IndexOf(".ca") > 0) ? "dianne_corp@abc.com" : "dc@abc.ca";

            if (vm1.Update() == 1)
            {
                vm2.Email = "something@different.com";  //we need something different
                Assert.True(vm2.Update() == -2);    //-2 = stale
            }
            else
                Assert.True(false);

        }


        [Fact]
        public void Call_GetAllTest()
        {
            CallViewModel vm = new CallViewModel();
            List<CallViewModel> allCallVms = vm.GetAll();
            Assert.True(allCallVms.Count > 0);
        }

        [Fact]
        public void Call_ComprehensiveVMTest()
        {
            CallViewModel cvm = new CallViewModel();
            EmployeeViewModel evm = new EmployeeViewModel();
            ProblemViewModel pvm = new ProblemViewModel();
            cvm.DateOpened = DateTime.Now;
            cvm.DateClosed = null;
            cvm.OpenStatus = true;
            evm.Email = "dc@abc.com";
            evm.GetByEmail();
            cvm.EmployeeId = evm.Id;
            evm.Email = "sc@abc.com";
            evm.GetByEmail();
            cvm.TechId = evm.Id;
            pvm.Description = "Memory Upgrade";
            pvm.GetByDescription();
            cvm.ProblemId = pvm.Id;
            cvm.Notes = "Corpuz has bad RAM. Burner to fix it";
            cvm.Add();
            output.WriteLine("New Call Generated    -   Id  = " + cvm.Id);
            /*int id = cvm.Id;    //need id for delete later
            cvm.GetById();*/
            cvm.Notes += "\n Ordered new RAM!";
            /*if (cvm.Update()==1)
            {
                output.WriteLine("Call was updated " + cvm.Notes);
            }
            else
            {
                output.WriteLine("Call was not updated ");
            }

            cvm.Notes = "Another change to comments that should not work";

            if(cvm.Update()==-2)
            {
                output.WriteLine("Call was not updated, data was stale ");
            }
            cvm = new CallViewModel
            {
                Id=id
            };//need to reset because of concurrency occur
            
            cvm.GetById();

            if(cvm.Delete()==1)
            {
                output.WriteLine("Call was deleted ");
            }
            else
            {
                output.WriteLine("Call was not deleted ");
            }*/
           // cvm.GetById();
           // Exception ex = Assert.Throws<NullReferenceException>(() => cvm.GetById());//should throw expected exception
           // Assert.Equal("Object reference not set to an instance of an object.", ex.Message);

        }


    }
}
