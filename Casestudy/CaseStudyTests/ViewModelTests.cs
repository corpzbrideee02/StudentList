using System;
using System.Collections.Generic;
using System.Text;
using HelpdeskViewModels;
using Xunit;

namespace CaseStudyTests
{
    public class ViewModelTests
    {
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
    }
}
