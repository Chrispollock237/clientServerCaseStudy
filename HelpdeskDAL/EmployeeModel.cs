/*
    Name:           EmployeeModel.cs       
    Author:         Chris Pollock
    Date:           October 26, 2018
    Purpose:        To create an EmployeeModel object which is an Employee Entity to hold all
                    of the data the relates to an Employee
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Entity.Infrastructure;

namespace HelpdeskDAL
{
    // EmployeeModel  class and the methods of that class
    public class EmployeeModel
    {
        // IRepository interface where the Employee is a HelpdeskEntity
        IRepository<Employee> repo;

        // EmployeetModel constructor, repo is a object of the HelpdeskRepository class
        public EmployeeModel()
        {
            repo = new HelpdeskRepository<Employee>();
        }

        // GetByEmail function that will retrieve an Employee by their email
        public Employee GetByEmail(string email)
        {
            // Create a list to hold the Employee objects
            List<Employee> selectedEmployee = null;

            try
            {
                // Trys to populate the List by checking the database to see if the Email 
                // past into the function exists in the database
                selectedEmployee = repo.GetByExpression(Employee => Employee.Email == email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Return the First instace of that Email found in the database
            return selectedEmployee.FirstOrDefault();
        }

        // GetBYLastName function that will retrieve an Employee by their last name
        public Employee GetByLastname(string name)
        {
            // Create a list to hold the Employee objects
            List<Employee> selectedEmployee = null;

            try
            {
                // Trys to populate the List by checking the database to see if the name 
                // past into the function exists in the database
                selectedEmployee = repo.GetByExpression(Student => Student.LastName == name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Return the First instace of that last name found in the database
            return selectedEmployee.FirstOrDefault();
        }

        // GetById functions to retrieve an Employee based on their Id 
        public Employee GetById(int id)
        {
            // Create a List to hold the Employee you are retrieving by Id
            List<Employee> selectedEmployee = null;

            try
            {
                // Trys to populate the List by checking the database to see if the Id 
                // past into the function exists in the database
                selectedEmployee = repo.GetByExpression(Employee => Employee.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Return the First instace of that Id found in the database
            return selectedEmployee.FirstOrDefault();
        }

        // GetAll() function to populate a List of all the Employee objects
        public List<Employee> GetAll()
        {
            // Create the List to hold all of the Employee objects
            List<Employee> allEmployees = new List<Employee>();

            try
            { 
                // Gets all of the Employees and adds the objects to the List
                allEmployees = repo.GetAll().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Return the List populated with the Employee objects
            return allEmployees;
        }

        // Add() function to Add an employee object to the database
        public int Add(Employee newEmployee)
        {
            try
            {
                // Try and Add the employee object to the database
                repo.Add(newEmployee); 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // If the Add was successful then return the new Id that Employee 
            // object will have
            return newEmployee.Id;
        }

        public int Update(Employee updatedEmployee)
        {
            int employeesUpdated = -1;

            try
            {
                HelpdeskContext ctx = new HelpdeskContext();
                Employee currentStudent = ctx.Employees.FirstOrDefault(Employee => Employee.Id == updatedEmployee.Id);
                ctx.Entry(currentStudent).CurrentValues.SetValues(updatedEmployee);
                employeesUpdated = ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return employeesUpdated;
        }

        // UpdateStatus to UpdateForConcurrency, this is to make sure the data the
        // user is using is not stale when they try and update an Employee
        public UpdateStatus UpdateForConcurrency(Employee updatedEmployee)
        {
            // Initially set to Failed because we haven't tried the Update yet
            UpdateStatus opStatus = UpdateStatus.Failed;

            try
            {
                // Try and Update the Employee, if the update is successful then will
                // change the status(an integer) to 1, and -2 if the data is stale 
                opStatus = repo.Update(updatedEmployee);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Return the Status(integer) to tell if the update was successful, failed,
            // or if the data was stale
            return opStatus;
        }

        // Delete function to delete an employee using the ID of the employee object
        public int Delete(int id)
        {
            // Number of employees deleted
            int employeesDeleted = -1;

            try
            {
                // Try and delete the employee from the database. Calls the delete
                // function from the HelpdeskRepository.
                employeesDeleted = repo.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Return the number of employees deleted (Should be 1)
            return employeesDeleted;
        }
    }
}
