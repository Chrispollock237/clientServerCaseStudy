using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelpdeskViewModels;

namespace HelpdeskWebsite.Controllers
{
    public class ProblemController : ApiController
    {
        [Route("api/problems")]
        // 'GetAll' method to get all of the employees in the database by creating
        // a EmployeeViewModel object and storing all the employee data for each employee
        // and adding the object to a List of EmployeeViewModel objects.
        public IHttpActionResult GetAll()
        {
            try
            {
                ProblemViewModel problem = new ProblemViewModel();
                List<ProblemViewModel> allProblems = problem.GetAll();
                return Ok(allProblems);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }
    }
}
