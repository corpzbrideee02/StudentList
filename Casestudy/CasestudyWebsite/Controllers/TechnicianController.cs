using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using HelpdeskViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CasestudyWebsite.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TechnicianController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAll()
        {
            try
            {
                EmployeeViewModel viewmodel = new EmployeeViewModel();
                List<EmployeeViewModel> allTechnician = viewmodel.GetAllTech();
                return Ok(allTechnician);
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
