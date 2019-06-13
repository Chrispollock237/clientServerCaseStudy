/*
    Name:           DepartmentController
    Author:         Chris Pollock
    Date:           October 26, 2018
    Purpose:        Contains the Route that will used the fetch the required data of the departments
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelpdeskViewModels;

namespace HelpdeskWebsite.Controllers
{
    // DepartmentController class that contains the method to get all the department data
    // from the database
    public class DepartmentController : ApiController
    {
        // Route that the fetch command in the employee.js will use to 
        // grab all of the department data from the database
        [Route("api/departments")]

        // GetAll() function to get all of the department data by creating 
        // a DepartmentViewModel object to hold all the department data and load that
        // data into a List. Will return an Ok status to say the data retrieval
        // was a success, otherwise it will return a BadRequest.
        public IHttpActionResult GetAll()
        {
            try
            {
                DepartmentViewModel dep = new DepartmentViewModel();
                List<DepartmentViewModel> allDepartments = dep.GetAll();
                return Ok(allDepartments);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }
    }
}
