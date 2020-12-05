
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
    public class GradeController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                GradeViewModel viewmodel = new GradeViewModel();
                List<GradeViewModel> allGrades= viewmodel.GetAll(id);
                return Ok(allGrades);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);    //something went wrong
            }
        }

        [HttpPut]
        public IActionResult Put(GradeViewModel viewmodel)
        {
            try
            {
                int retVal = viewmodel.Update();
                return retVal switch
                {
                    1 => Ok(new { msg = "Grade "  + " updated for Student " + viewmodel.StudentId }),
                    -1 => Ok(new { msg = "Grade " + " not updated for Student " + viewmodel.StudentId }),
                    -2 => Ok(new { msg = "Data is stale for Student " + viewmodel.StudentId + " not updated!" }),
                    _ => Ok(new { msg = "Grade for Student " + viewmodel.StudentId + " not updated!" }),
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                   MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);    //something went wrong
            }
        }
        [HttpGet]
        public IActionResult GetAll(int id)
        {
            try
            {
               GradeViewModel viewmodel = new GradeViewModel();
                List<GradeViewModel> allGrades = viewmodel.GetAll(id);
                return Ok(allGrades);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);    //something went wrong
            }
        }


        /*[HttpPost]
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
        }*/

       /* [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                GradeViewModel viewmodel = new GradeViewModel { Id = id };
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
        }*/


    }
}
