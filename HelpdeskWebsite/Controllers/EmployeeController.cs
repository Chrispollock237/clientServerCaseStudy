/*
    Name:           EmployeeController.cs
    Author:         Chris Pollock
    Date:           October 26, 2018
    Purpose:        Contains the methods the Add, Delete, Retrieve, and Update an employee
                    from the database
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
    // EmployeeController class to hold the methods that will be called in the JavaScript
    // to Add, Delete, or Update an employee
    public class EmployeeController : ApiController
    {
        // Route that the fetch command in the employee.js will use to 
        // get the employee data by the Lastname of the employee 
        [Route("api/employees/{name}")]
        public IHttpActionResult Get(string name)
        {
            try
            {
                // Create a EmployeeViewModel object and pass it the name
                // that will be used to GetByLastName(), will return Ok
                // if lastname is in the database
                EmployeeViewModel emp = new EmployeeViewModel();
                emp.Lastname = name;
                emp.GetByLastname();
                return Ok(emp);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }

        // Route that the fetch command in the employee.js
        [Route("api/employees")]

        // 'PUT' method to be called when the user wants to update an employee that
        // is already in the database.
        public IHttpActionResult Put(EmployeeViewModel emp)
        {
            try
            {
                // Retrieves the update status from the EmployeeViewModel, to say whether the update
                // was good, didn't happen or that the user had stale data which also would not update
                // the employee information. Then displays a message based on the value returned, saying
                // whether or not the update happened.
                int retVal = emp.Update();
                switch (retVal)
                {
                    case 1:
                        return Ok("Employee " + emp.Lastname + " updated!");
                    case -1:
                        return Ok("Employee " + emp.Lastname + " not update!");
                    case -2:
                        return Ok("Data is stale for " + emp.Lastname + ", Employee not update!");
                    default:
                        return Ok("Employee " + emp.Lastname + " not update!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("update failed - " + ex.Message);
            }
        }

        // Route that the fetch command in the employee.js
        [Route("api/employees")]

        // 'GetAll' method to get all of the employees in the database by creating
        // a EmployeeViewModel object and storing all the employee data for each employee
        // and adding the object to a List of EmployeeViewModel objects.
        public IHttpActionResult GetAll()
        {
            try
            {
                EmployeeViewModel emp = new EmployeeViewModel();
                List<EmployeeViewModel> allEmployees = emp.GetAll();
                return Ok(allEmployees);
            }
            catch (Exception ex)
            {
                return BadRequest("Retrieve failed - " + ex.Message);
            }
        }

        // Route that the fetch command in the employee.js
        [Route("api/employees")]

        // 'POST' method that adds a employee to the database
        public  IHttpActionResult Post(EmployeeViewModel emp)
        {
            try
            {
                // Calls the EmployeeViewModel Add() function to add the required
                // employee data
                emp.Add();

                // If an employee has been successfully added then display message saying
                // the add was successful, otherwise display that it was unsuccessful
                if (emp.Id > 0)
                {
                    return Ok("Student " + emp.Lastname + " added!");
                }
                else
                {
                    return Ok("Student " + emp.Lastname + " not added!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Creating failed - Contact Tech Support");
            }
        }

        // Route that the fetch command in the employee.js to get the departments by ID
        [Route("api/departments/{id}")]

        // 'DELETE' method to remove and employee from the database
        public IHttpActionResult Delete(int id)
        {
            try
            {
                // Create and EmployeeViewModel object to store the employee data
                EmployeeViewModel emp = new EmployeeViewModel();

                // The EmployeeViewModel Object ID is that of the ID past into the methods parameters
                // when called in the JavaScript
                emp.Id = id;

                // If the number of employees deleted equals 1 then display the message that it was successful
                // else display a message that it was unsuccessful
                if (emp.Delete() == 1)
                {
                    return Ok("Employee " + emp.Id + " deleted!");
                }
                else
                {
                    return Ok("Employee " + emp.Id + " not deleted!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Delete failed - Contact Tech Support");
            }
        }
    }
}
