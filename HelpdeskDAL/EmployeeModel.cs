using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Entity.Infrastructure;

namespace HelpdeskDAL
{
    public class EmployeeModel
    {
        IRepository<Employee> repo;

        public EmployeeModel()
        {
            repo = new HelpdeskRepository<Employee>();
        }

        public Employee GetByEmail(string email)
        {
            List<Employee> selectedEmployee = null;

            try
            {
                selectedEmployee = repo.GetByExpression(Employee => Employee.Email == email);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return selectedEmployee.FirstOrDefault();
        }

        public Employee GetByLastname(string name)
        {
            List<Employee> selectedEmployee = null;

            try
            {
                selectedEmployee = repo.GetByExpression(Student => Student.LastName == name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return selectedEmployee.FirstOrDefault();
        }

        public Employee GetById(int id)
        {
            List<Employee> selectedEmployee = null;

            try
            {
                selectedEmployee = repo.GetByExpression(Employee => Employee.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return selectedEmployee.FirstOrDefault();
        }

        public List<Employee> GetAll()
        {
            List<Employee> allEmployees = new List<Employee>();

            try
            {
                allEmployees = repo.GetAll().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            return allEmployees;
        }

        public int Add(Employee newEmployee)
        {
            try
            {
                repo.Add(newEmployee); 
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

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

        public UpdateStatus UpdateForConcurrency(Employee updatedEmployee)
        {
            UpdateStatus opStatus = UpdateStatus.Failed;
            try
            {
                opStatus = repo.Update(updatedEmployee);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return opStatus;
        }

        public int Delete(int id)
        {
            int employeesDeleted = -1;

            try
            {
                employeesDeleted = repo.Delete(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
            return employeesDeleted;
        }
    }
}
