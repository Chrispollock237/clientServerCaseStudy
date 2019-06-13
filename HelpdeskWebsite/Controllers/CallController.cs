using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelpdeskViewModels;

namespace HelpdeskWebsite.Controllers
{
    public class CallController : ApiController
    {
        // Route that the fetch command in the call.js
        [Route("api/calls")]

        // 'GetAll' method to get all of the employees in the database by creating
        // a EmployeeViewModel object and storing all the employee data for each employee
        // and adding the object to a List of EmployeeViewModel objects.
        public IHttpActionResult GetAll()
        {
            try
            {
                CallViewModel call = new CallViewModel();
                List<CallViewModel> allCalls = call.GetAll();
                return Ok(allCalls);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }

        // Route that the fetch command in the call.js
        [Route("api/calls")]

        // 'PUT' method to be called when the user wants to update an employee that
        // is already in the database.
        public IHttpActionResult Put(CallViewModel call)
        {
            try
            {
                // Retrieves the update status from the EmployeeViewModel, to say whether the update
                // was good, didn't happen or that the user had stale data which also would not update
                // the employee information. Then displays a message based on the value returned, saying
                // whether or not the update happened.
                int retVal = call.Update();
                switch (retVal)
                {
                    case 1:
                        return Ok("Call " + call.Id + " updated!");
                    case -1:
                        return Ok("Call " + call.Id + " not update!");
                    case -2:
                        return Ok("Data is stale for " + call.Id + ", Call not update!");
                    default:
                        return Ok("Call " + call.Id + " not update!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("update failed - " + ex.Message);
            }
        }

        // Route that the fetch command in the calls.js
        [Route("api/calls")]

        // 'POST' method that adds a call to the database
        public IHttpActionResult Post(CallViewModel call)
        {
            try
            {
                // Calls the EmployeeViewModel Add() function to add the required
                // employee data
                call.Add();

                // If an call has been successfully added then display message saying
                // the add was successful, otherwise display that it was unsuccessful
                if (call.Id > 0)
                {
                    return Ok("Call " + call.Id + " added!");
                }
                else
                {
                    return Ok("Call " + call.Id + " not added!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Creating failed - Contact Tech Support");
            }
        }

        [Route("api/calls/{id}")]

        // 'DELETE' method to remove and employee from the database
        public IHttpActionResult Delete(int id)
        {
            try
            {
                // Create and EmployeeViewModel object to store the employee data
                CallViewModel call = new CallViewModel();

                // The EmployeeViewModel Object ID is that of the ID past into the methods parameters
                // when called in the JavaScript
                call.Id = id;

                // If the number of employees deleted equals 1 then display the message that it was successful
                // else display a message that it was unsuccessful
                if (call.Delete() == 1)
                {
                    return Ok("Call " + call.Id + " deleted!");
                }
                else
                {
                    return Ok("Call " + call.Id + " not deleted!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Delete failed - Contact Tech Support");
            }
        }
    }
}