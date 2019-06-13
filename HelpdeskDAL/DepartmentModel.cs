/*
    Name:           DepartmentModel.cs
    Author:         Chris Pollock
    Date:           October 26, 2018
    Purpose:        To create a DepartmentModel object that will then get an ID and a Timer from 
                    from the HelpdeskEntity. The DepartmentModel objects will then be added to
                    a List of all the departments.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Entity.Infrastructure;

namespace HelpdeskDAL
{
    // DepartmentModel class and the methods of that class
    public class DepartmentModel
    {
        // IRepository interface where the Department is a HelpdeskEntity
        IRepository<Department> repo;

        // DepartmentModel constructor, repo is a object of the HelpdeskRepository class
        public DepartmentModel()
        {
            repo = new HelpdeskRepository<Department>();
        }

        // GetAll() method that gets all the department objects and sets and adds them to a List
        // of Department objects
        public List<Department> GetAll()
        {
            // List to hold the Department objects
            List<Department> allDepartments = new List<Department>();

            try
            {
                // List the gets populated by Department objects
                // Calls the GetAll() from the HelpdeskRepository which uses a HelpdeskContext
                // to create the Department object to hold the data
                allDepartments = repo.GetAll().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Returns the List of the Department objects
            return allDepartments;
        }
    }
}
