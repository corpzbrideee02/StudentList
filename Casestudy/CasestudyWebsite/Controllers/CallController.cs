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
    public class CallController : ControllerBase
    {

        [HttpPut]
        public ActionResult Put(CallViewModel viewmodel)
        {
            try
            {
                int retVal = viewmodel.Update();
                return retVal switch
                {
                    1 => Ok(new { msg = "Call " + viewmodel.Id+ " updated!" }),
                    -1 => Ok(new { msg = "Call " + viewmodel.Id + " not updated!" }),
                    -2 => Ok(new { msg = "Data is stale for " + viewmodel.Id + "Call not updated!" }),
                    _ => Ok(new { msg = "Employee " + viewmodel.Id + " not updated!" }),
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
        public IActionResult GetAll()
        {
            try
            {
                CallViewModel viewmodel = new CallViewModel();
                List<CallViewModel> allEmployee = viewmodel.GetAll();
                return Ok(allEmployee);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);    //something went wrong
            }
        }

        [HttpPost]
        public ActionResult Post(CallViewModel viewmodel)
        {
            try
            {
                viewmodel.Add();
                return viewmodel.Id > 1
                    ? Ok(new { msg = "Call " + viewmodel.Id + " added!" })
                    : Ok(new { msg = "Call " + viewmodel.Id + " not added!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);    //something went wrong
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                CallViewModel viewmodel = new CallViewModel { Id = id };
                return viewmodel.Delete() == 1
                      ? Ok(new { msg = "Call " + id + " deleted!" })
                    : Ok(new { msg = "Call" + id + " not deleted!" });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);    //something went wrong
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                CallViewModel viewmodel = new CallViewModel();
                List<CallViewModel> allCalls = viewmodel.GetAll();
                return Ok(allCalls);
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
