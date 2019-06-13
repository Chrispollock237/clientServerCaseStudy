/*
    Name:           EmployeeViewModel.cs       
    Author:         Chris Pollock
    Date:           October 26, 2018
    Purpose:        Contains a class that will contain the Employee Title, First name,
                    Last name, Email, Phone number, Department Id, Department name, Picture
                    and a Timer for concurrency related to a specific employee
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpdeskDAL;
using System.Reflection;

namespace HelpdeskViewModels
{
    // EmployeeViewModel holds the  Employee Title, First name,
    // Last name, Email, Phone number, Department Id, Department name, Picture
    // and a Timer values
    public class EmployeeViewModel
    {
        private EmployeeModel _model;
        public string Title { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Phoneno { get; set; }
        public string Timer { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int Id { get; set; }
        public bool? IsTech { get; set; }
        public string StaffPicture64 { get; set; }

        // Constructor to create a new EmployeeModel object
        public EmployeeViewModel()
        {
            _model = new EmployeeModel();
        }

        // Get an employee using the Lastname property
        public void GetByLastname()
        {
            try
            {
                // New Employee object that will be created with the Lastname
                Employee emp = _model.GetByLastname(Lastname);
                Title = emp.Title;
                Firstname = emp.FirstName;
                Lastname = emp.LastName;
                Phoneno = emp.PhoneNo;
                Email = emp.Email;
                Id = emp.Id;
                DepartmentId = emp.DepartmentId;
                IsTech = emp.IsTech;
                if (emp.StaffPicture != null)
                {
                    StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                }
                Timer = Convert.ToBase64String(emp.Timer);
            }
            catch (Exception ex)
            {
                Lastname = "not found";
                Console.WriteLine("Problem in" + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        // Retrieve all the EmployeeViewModel objects and add them to a List
        public List<EmployeeViewModel> GetAll()
        {
            // Create a list to hold the EmployeeViewModel objects
            List<EmployeeViewModel> allVms = new List<EmployeeViewModel>();
            try
            {
                // Create the List to hold the Employee objects. Uses the GetAll()
                // from the EmployeeModel class to populate this List
                List<Employee> allEmployees = _model.GetAll();

                // For each Employee in the allEmployees List create a view model for them and 
                // then add them to the List of Employee objects
                foreach (Employee emp in allEmployees)
                {
                    EmployeeViewModel empVm = new EmployeeViewModel();
                    empVm.Title = emp.Title;
                    empVm.Firstname = emp.FirstName;
                    empVm.Lastname = emp.LastName;
                    empVm.Phoneno = emp.PhoneNo;
                    empVm.Email = emp.Email;
                    empVm.Id = emp.Id;
                    empVm.DepartmentId = emp.DepartmentId;
                    empVm.DepartmentName = emp.Department.DepartmentName;
                    empVm.IsTech = emp.IsTech;
                    if (emp.StaffPicture != null)
                    {
                        empVm.StaffPicture64 = Convert.ToBase64String(emp.StaffPicture);
                    }
                    empVm.Timer = Convert.ToBase64String(emp.Timer);
                    allVms.Add(empVm);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Return the List of EmployeeViewModel objects 
            return allVms;
        }

        //Add Employee function
        public void Add()
        {
            Id = -1;
            try
            {
                // Create a new Employee object and add the properties
                // from the object that has called the Add() function
                Employee emp = new Employee();
                emp.Title = Title;
                emp.FirstName = Firstname;
                emp.LastName = Lastname;
                emp.PhoneNo = Phoneno;
                emp.Email = Email;
                emp.DepartmentId = DepartmentId;
                Id = _model.Add(emp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                   MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        // Update an Employee
        public int Update()
        {
            // UpdatesStatus is set to failed initally until we can check 
            // for data concurrency to make sure the data is not stale
            UpdateStatus opStatus = UpdateStatus.Failed;
            try
            {
                // Create a new Employee object and add the properties
                //  from the object that has called the Update() function
                Employee emp = new Employee();
                emp.Title = Title;
                emp.FirstName = Firstname;
                emp.LastName = Lastname;
                emp.PhoneNo = Phoneno;
                emp.Email = Email;
                emp.Id = Id;
                emp.DepartmentId = DepartmentId;
                if (StaffPicture64 != null)
                {
                    emp.StaffPicture = Convert.FromBase64String(StaffPicture64);
                }
                emp.Timer = Convert.FromBase64String(Timer);

                // This is where we check for the data concurrency to make sure that data 
                // is not stale
                opStatus = _model.UpdateForConcurrency(emp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                     MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return Convert.ToInt16(opStatus);
        }

        // Delete a Employee
        public int Delete()
        {
            // Sets the number of Employees deleted 
            // None of been deleted until we try and actually remove them 
            // from the database
            int EmployeesDeleted = -1;

            try
            {
                // Trys to delete the Employee from the database
                // The Id is past from the EmployeeController from the object
                // that called the inital Delete function
                EmployeesDeleted = _model.Delete(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                     MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Returns the number of employees deleted
            return EmployeesDeleted;
        }

        // Find Employee using Id property
        public void GetById()
        {
            try
            {
                // Create Employee object to hold the data of the object
                // calling the function
                Employee emp = new Employee();
                emp.Title = Title;
                emp.FirstName = Firstname;
                emp.LastName = Lastname;
                emp.PhoneNo = Phoneno;
                emp.Email = Email;
                emp.Id = Id;
                emp.DepartmentId = DepartmentId;
                if (StaffPicture64 != null)
                {
                    emp.StaffPicture = Convert.FromBase64String(StaffPicture64);
                }
                emp.Timer = Convert.FromBase64String(Timer);
            }
            catch (NullReferenceException nex)
            {
                Lastname = "Not Found";
            }
            catch (Exception ex)
            {
                Lastname = "Not Found";
                Console.WriteLine("Problem in " + GetType().Name + " " +
                     MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
    }
}
