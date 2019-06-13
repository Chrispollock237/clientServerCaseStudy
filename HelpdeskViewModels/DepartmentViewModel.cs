/*
    Name:           DepartmentViewModel.cs       
    Author:         Chris Pollock
    Date:           October 26, 2018
    Purpose:        Contains a class that will contains the Department ID, Name, and a Timer
                    for concurrency related to a specific employee
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
    // DepartmentViewModel class which will hold the Department Id, Name, and a Timer
    public class DepartmentViewModel
    {
        // Variables to hold specific information of the Department
        private DepartmentModel _model;
        public int Id { get; set; }
        public string Name { get; set; }
        public string Timer { get; set; }

        // DepartmentViewModel() constructor
        public DepartmentViewModel()
        {
            _model = new DepartmentModel();
        }

        // GetAll() function of the DepartmentViewModel class
        public List<DepartmentViewModel> GetAll()
        {
            // List to hold all the DepartmentViewModel objects
            List<DepartmentViewModel> allVms = new List<DepartmentViewModel>();
            try
            {
                // List to hold all the departments in the database
                List<Department> allDepartments = _model.GetAll();

                // For each department in the database  it will create a DepartmentViewModel
                // object to store the Department Name, Id and a Timer for concurrency issues
                // then adds the DepartmentViewModel object to the List
                foreach (Department dep in allDepartments)
                {
                    DepartmentViewModel depVm = new DepartmentViewModel();
                    depVm.Name = dep.DepartmentName;
                    depVm.Id = dep.Id;
                    depVm.Timer = Convert.ToBase64String(dep.Timer);
                    allVms.Add(depVm);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            
            // Return the List of DepartmentViewModel objects
            return allVms;
        }
    }
}
