using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using ExercisesViewModels;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ExercisesWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet("{lastname}")]
        public IActionResult GetByLastname(string lastname)
        {
            try
            {
                StudentViewModel viewmodel = new StudentViewModel();
                viewmodel.Lastname = lastname;
                viewmodel.GetByLastname();
                return Ok(viewmodel);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name+" "+ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);    //something went wrong
            }
        }

        [HttpPut]
        public ActionResult Put(StudentViewModel viewmodel)
        {
            try
            {
                int retVal = viewmodel.Update();
                return retVal switch
                {
                    1=>Ok(new { msg="Student "+viewmodel.Lastname+" updated!"}),
                    -1 => Ok(new { msg = "Student " + viewmodel.Lastname + " not updated!" }),
                    -2=> Ok(new { msg = "Data is stale for " + viewmodel.Lastname + "Student not updated!" }),
                    _=> Ok(new { msg = "Student " + viewmodel.Lastname + " not updated!" }),
                };
            }
            catch(Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                   MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);    //something went wrong
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                StudentViewModel viewmodel = new StudentViewModel();
                List<StudentViewModel> allStudents = viewmodel.GetAll();
                return Ok(allStudents);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);    //something went wrong
            }
        }
        [HttpPost]
        public ActionResult Post(StudentViewModel viewmodel)
        {
            try
            {
                viewmodel.Add();
                return viewmodel.Id > 1
                    ? Ok(new { msg = "Student " + viewmodel.Lastname + " added!" })
                    : Ok(new { msg = "Student " + viewmodel.Lastname + " not added!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);    //something went wrong
            }
        }

        [HttpDelete("{id")]
        public IActionResult Delete(int id)
        {
            try
            {
                StudentViewModel viewmodel = new StudentViewModel { Id = id };
                return viewmodel.Delete() == 1
                      ? Ok(new { msg = "Student " + id + " deleted!" })
                    : Ok(new { msg = "Student " + id + " not deleted!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);    //something went wrong
            }
        }


    }
}
