
using Xunit;
using System;
using System.Collections.Generic;
using System.Text;
using ExercisesDAL;
using System.Diagnostics;
using System.Data;

namespace ExerciseTests
{
    public class DAOTests
    {
        [Fact]
        public void Student_GetByLastnameTest()
        {
            try
            {
                StudentDAO dao = new StudentDAO();
                Students selectedStudent = dao.GetByLastName("Pet");
                Assert.NotNull(selectedStudent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }

        }

        [Fact]
        public void Student_GetByIdTest()
        {
            try
            {
                StudentDAO dao = new StudentDAO();
                Students selectedStudent = dao.GetById(2);
                Assert.NotNull(selectedStudent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }

        }
        [Fact]
        public void Student_GetAllTest()
        {
            try
            {
                StudentDAO dao = new StudentDAO();
                List<Students> allStudents = dao.GetAll();
                Assert.True(allStudents.Count > 0);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }

        }

        [Fact]
        public void Student_AddTest()
        {
            try
            {
                StudentDAO dao = new StudentDAO();
                Students student = new Students();
                Students newStudent = new Students
                {
                    FirstName = "Joe",
                    LastName = "Smith",
                    PhoneNo = "(555)555-1234",
                    Title = "Mr.",
                    DivisionId = 10,
                    Email = "el@someschool.com"
                };
                Assert.True(dao.Add(newStudent) > 0);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }

        }

        [Fact]
        public void Student_UpdateTest()
        {
            try
            {
                StudentDAO dao = new StudentDAO();
                Students studentForUpdate = dao.GetByLastName("Smith");

                if (studentForUpdate != null)
                {
                    string oldPhoneNo = studentForUpdate.PhoneNo;
                    string newPhoneNo = oldPhoneNo == "519-555-1234" ? "555-555-5555" : "519-555-1234";
                    studentForUpdate.PhoneNo = newPhoneNo;
                }
                Assert.True(dao.Update(studentForUpdate) == ExercisesDAL.UpdateStatus.Ok);   //indicates the #of rows updated
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }
        }

        [Fact]
        public void Student_ConcurrencyTest()
        {
            StudentDAO dao1 = new StudentDAO();
            StudentDAO dao2 = new StudentDAO();
            Students studentForUpdate1 = dao1.GetByLastName("Smith");
            Students studentForUpdate2 = dao2.GetByLastName("Smith");

            if (studentForUpdate1 != null)
            {
                string oldPhoneNo = studentForUpdate1.PhoneNo;
                string newPhoneNo = oldPhoneNo == "519-555-1234" ? "555-555-5555" : "519-555-1234";
                studentForUpdate1.PhoneNo = newPhoneNo;
                if (dao1.Update(studentForUpdate1) == ExercisesDAL.UpdateStatus.Ok)
                {
                    //need to change the phone # to something else
                    studentForUpdate2.PhoneNo = "666-666-6666";
                    Assert.True(dao2.Update(studentForUpdate2) == ExercisesDAL.UpdateStatus.Stale);

                }
                else
                    Assert.True(false);

            }

        }


        [Fact]
        public void Student_DeleteTest()
        {
            try
            {
                StudentDAO dao = new StudentDAO();
                Students studentForDelete = dao.GetByLastName("Smith");

                Assert.True(dao.Delete(studentForDelete.Id) == 1);   //indicates the #of rows deleted
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error - " + ex.Message);
            }

        }

        [Fact]
        public void Student_LoadPicsTest()
        {
            DALUtil util = new DALUtil();
            Assert.True(util.AddStudentPicsToDb());

        }
    }



  }
