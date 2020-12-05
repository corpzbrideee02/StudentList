
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
    public class CourseController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            try
            {
                CourseViewModel viewmodel = new CourseViewModel();
                List<CourseViewModel> allCourses = viewmodel.GetAll(id);

                return Ok(allCourses);
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
               CourseViewModel viewmodel = new CourseViewModel();
                List<CourseViewModel> allCourses = viewmodel.GetAll(id);
         
                return Ok(allCourses);
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
