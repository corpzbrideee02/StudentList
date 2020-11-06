using System;
using System.Collections.Generic;
using System.Text;
using ExercisesViewModels;
using Xunit;

namespace ExerciseTests
{
    public class ViewModelTests
    {
        [Fact]
        public void Student_GetByLastNameTest()
        {
            StudentViewModel vm = new StudentViewModel { Lastname = "Pet" };
            vm.GetByLastname();
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public void Student_GetByIdTest()
        {
            StudentViewModel vm = new StudentViewModel { Lastname = "Pet" };
            vm.GetByLastname();
            vm.GetById();
            Assert.NotNull(vm.Firstname);
        }

        [Fact]
        public void Student_GetAllTest()
        {
            StudentViewModel vm = new StudentViewModel();
            List<StudentViewModel> allStudentVms = vm.GetAll();
            Assert.True(allStudentVms.Count > 0);
        }

        [Fact]
        public void Student_AddTest()
        {
            StudentViewModel vm = new StudentViewModel()
            {
                Title = "Ms.",
                Firstname="Dianne",
                Lastname="Corpuz",
                Email="dc@abc.com",
                Phoneno="(555)555-5551",
                DivisionId=10
            };
            vm.Add();
            Assert.True(vm.Id > 0);
        }
        [Fact]
        public void Student_UpdateTest()
        {
            StudentViewModel vm = new StudentViewModel { Lastname = "Corpuz" };
            vm.GetByLastname();//Student just added
            vm.Phoneno = vm.Phoneno == "(555)555-5551" ? "(555)555-5552" : "(555)555-5551";
            int StudentsUpdated = vm.Update();
           Assert.True(StudentsUpdated > 0);
            
        }
        [Fact]
        public void Student_DeleteTest()
        {
            StudentViewModel vm = new StudentViewModel { Lastname = "Corpuz" };
            vm.GetByLastname();//Student just added
             int StudentsDeleted = vm.Delete();
            Assert.True(StudentsDeleted ==1);
        }

        [Fact]
        public void Student_ConcurrencyTest()
        {
            StudentViewModel vm1 = new StudentViewModel();
            StudentViewModel vm2 = new StudentViewModel();
            vm1.Lastname = "Corpuz";
            vm2.Lastname = "Corpuz";

            vm1.GetByLastname();
            vm2.GetByLastname();
            vm1.Email=(vm1.Email.IndexOf(".ca")>0)?"dc@abc.com": "dc@abc.ca";

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
