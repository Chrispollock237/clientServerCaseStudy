using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpdeskDAL;
using System.Reflection;



namespace HelpdeskViewModels
{
    public class CallViewModel
    {
        private CallModel _model;
        public int EmployeeId { get; set; }
        public int ProblemId { get; set; }
        public int TechId { get; set; }
        public int Id { get; set; }
        public DateTime DateOpened { get; set; }
        public DateTime? DateClosed { get; set; }
        public bool OpenStatus { get; set; }
        public string Notes { get; set; }
        public string Timer { get; set; }

        public string EmployeeName { get; set; }
        public string TechName { get; set; }
        public string ProblemDescription { get; set; }


        public CallViewModel()
        {
            _model = new CallModel();
        }

        // Retrieve all the EmployeeViewModel objects and add them to a List
        public List<CallViewModel> GetAll()
        {
            // Create a list to hold the EmployeeViewModel objects
            List<CallViewModel> allVms = new List<CallViewModel>();
            try
            {
                // Create the List to hold the Employee objects. Uses the GetAll()
                // from the EmployeeModel class to populate this List
                List<Call> allCalls = _model.GetAll();

                // For each Employee in the allEmployees List create a view model for them and 
                // then add them to the List of Employee objects
                foreach (Call call in allCalls)
                {
                    CallViewModel callVm = new CallViewModel();
                    callVm.Id = call.Id;
                    callVm.EmployeeId = call.EmployeeId;
                    callVm.ProblemId = call.ProblemId;
                    callVm.TechId = call.TechId;
                    callVm.DateOpened = call.DateOpened;
                    callVm.DateClosed = call.DateClosed;
                    callVm.OpenStatus = call.OpenStatus;
                    callVm.Notes = call.Notes;
                    callVm.Timer = Convert.ToBase64String(call.Timer);

                    callVm.EmployeeName = call.Employee.FirstName + " " + call.Employee.LastName;
                    callVm.TechName = call.Employee1.FirstName + " " + call.Employee1.LastName;
                    callVm.ProblemDescription = call.Problem.Description;

                    allVms.Add(callVm);
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

        //Add Call function
        public void Add()
        {
            Id = -1;
            try
            {
                // Create a new Call object and add the properties
                // from the object that has called the Add() function
                Call call = new Call();
                call.EmployeeId = EmployeeId;
                call.ProblemId = ProblemId;
                call.TechId = TechId;
                call.DateOpened = DateOpened;
                call.OpenStatus = OpenStatus;
                call.DateClosed = DateClosed;
                call.Notes = Notes;
                //call.Timer = Convert.FromBase64String(Timer);
                Id = _model.Add(call);
                Timer = Convert.ToBase64String(call.Timer);
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
                // Create a new Call object and add the properties
                // from the object that has called the Add() function
                Call call = new Call();
                call.EmployeeId = EmployeeId;
                call.ProblemId = ProblemId;
                call.TechId = TechId;
                call.DateOpened = DateOpened;
                call.OpenStatus = OpenStatus;
                call.DateClosed = DateClosed;
                call.Notes = Notes;
                call.Id = Id;
                call.Timer = Convert.FromBase64String(Timer);

                // This is where we check for the data concurrency to make sure that data 
                // is not stale
                opStatus = _model.Update(call);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                     MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return Convert.ToInt16(opStatus);
        }

        // Delete a Call
        public int Delete()
        {
            // Sets the number of Employees deleted 
            // None of been deleted until we try and actually remove them 
            // from the database
            int CallsDeleted = -1;

            try
            {
                // Trys to delete the Call from the database
                // The Id is past from the EmployeeController from the object
                // that called the inital Delete function
                CallsDeleted = _model.Delete(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                     MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Returns the number of employees deleted
            return CallsDeleted;
        }

        // Find Call using Id property
        public void GetById()
        {
            try
            {
                // Create a new Call object and add the properties
                // from the object that has called the Add() function
                Call call = _model.GetById(Id);
                EmployeeId = call.EmployeeId;
                ProblemId = call.ProblemId;
                TechId = call.TechId;
                DateOpened = call.DateOpened;
                OpenStatus = call.OpenStatus;
                DateClosed = call.DateClosed;
                Notes = call.Notes;
                Id = call.Id;

                Timer = Convert.ToBase64String(call.Timer);
            }
            catch (NullReferenceException nex)
            {
                Notes = "Not Found";
                throw nex;
            }
            catch (Exception ex)
            {
                Notes = "Not Found";
                Console.WriteLine("Problem in " + GetType().Name + " " +
                     MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }
    }
}


// callVm.Problem =  call.Problem.Description
// callVm.Tech = call.Employee1.FirstName + " " + call.Employee1LastName
// callVm.Employee = call.Employee.FirstName + " " + call.Employee.LastName
// Employee1 is TechID