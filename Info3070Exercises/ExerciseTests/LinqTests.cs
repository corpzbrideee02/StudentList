using System;
using ExercisesDAL;
using System.Linq;
using Xunit;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;

namespace ExerciseTests
{
    public class LinqTests
    {
        [Fact]
        public void Test1()
        {
            try
            {
                SomeSchoolContext _db = new SomeSchoolContext();
                var selectedStudents = from stu in _db.Students
                                       where stu.Id == 2
                                       select stu;
                Assert.True(selectedStudents.Count() > 0);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error- " + ex.Message);
            }
            
        }
        [Fact]
        public void Test2()
        {
            try
            {
                SomeSchoolContext _db = new SomeSchoolContext();
                var selectedStudents = from stu in _db.Students
                                       where stu.Title == "Ms." || stu.Title == "Mrs."
                                       select stu;
                Assert.True(selectedStudents.Count() > 0);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error- " + ex.Message);
            }

        }

        [Fact]
        public void Test3()
        {
            try
            {
                SomeSchoolContext _db = new SomeSchoolContext();
                var selectedStudents = from stu in _db.Students
                                        join div in _db.Divisions
                                        on stu.DivisionId equals div.Id
                                        where div.Name == "Design"
                                        select stu;
                /*var selectedStudents = from stu in _db.Students
                                       where stu.Division.Name == "Design"
                                       select stu;*/

                Assert.True(selectedStudents.Count() > 0);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error- " + ex.Message);
            }

        }
        [Fact]
        public void Test4()
        {
            try
            {
                SomeSchoolContext _db = new SomeSchoolContext();
                Students selectedStudents = _db.Students.FirstOrDefault(stu => stu.Id == 2);
                Assert.True(selectedStudents.FirstName=="Teachers");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error- " + ex.Message);
            }

        }
        [Fact]
        public void Test5()
        {
            try
            {
                SomeSchoolContext _db = new SomeSchoolContext();
                var selectedStudents = _db.Students.Where(stu => stu.Title == "Ms." || stu.Title == "Mrs.");
                Assert.True(selectedStudents.Count() > 0);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error- " + ex.Message);
            }

        }
        [Fact]
        public void Test6()
        {
            try
            {
                SomeSchoolContext _db = new SomeSchoolContext();
                var selectedStudents = _db.Students.Where(stu => stu.Division.Name=="Design");
                Assert.True(selectedStudents.Count() > 0);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error- " + ex.Message);
            }

        }
        [Fact]
        public void Test7() //CRUD Update Test
        {
            try
            {
                SomeSchoolContext _db = new SomeSchoolContext();
                var selectedStudents = _db.Students.FirstOrDefault(stu => stu.Id == 14);
                
                if(selectedStudents!=null)
                {
                    string oldEmailAdd = selectedStudents.Email;
                    string newEmailAdd = oldEmailAdd == "dc@someschool.com" ? "dmc@someschool.com" : "dc@someschool.com";
                    selectedStudents.Email = newEmailAdd;
                    _db.Entry(selectedStudents).CurrentValues.SetValues(selectedStudents);
                
                }
                
                Assert.True(_db.SaveChanges()==1);  //1 indicates that # of rows updated
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error- " + ex.Message);
            }

        }

        [Fact]
        public void Test8() //CRUD Create Test
        {
            try
            {
                SomeSchoolContext _db = new SomeSchoolContext();
                Students newStudent = new Students
                {
                    FirstName = "Dianne",
                    LastName = "Corpuz",
                    PhoneNo="(555)555-5550",
                    Title="Ms.",
                    DivisionId=10,
                    Email="dc@someschool.com"
               };
                _db.Students.Add(newStudent);
                _db.SaveChanges();

                Assert.True(newStudent.Id>1);  //should be poupulated after SaveChanges
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error- " + ex.Message);
            }

        }
        [Fact]
        public void Test9() //Delete with name "Joe"
        {
            try
            {
                SomeSchoolContext _db = new SomeSchoolContext();
                Students selectedStudents = _db.Students.FirstOrDefault(stu => stu.FirstName == "Joe" );
                if(selectedStudents!=null)
                {
                    _db.Students.Remove(selectedStudents);
                    Assert.True(_db.SaveChanges() == 1); //# of rows deleted
                }
                else
                {
                    Assert.True(false);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error- " + ex.Message);
            }

        }

        

    }
}
