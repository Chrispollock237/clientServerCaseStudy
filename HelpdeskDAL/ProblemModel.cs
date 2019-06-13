using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Entity.Infrastructure;

namespace HelpdeskDAL
{
    public class ProblemModel
    {
        // IRepository interface where the Problem is a HelpdeskEntity
        IRepository<Problem> repo;

        public ProblemModel()
        {
            repo = new HelpdeskRepository<Problem>();
        }

        public Problem GetByDescription(string desc)
        {
            // Create a list to hold the Employee objects
            List<Problem> selectedProblem = null;

            try
            {
                // Trys to populate the List by checking the database to see if the Email 
                // past into the function exists in the database
                selectedProblem = repo.GetByExpression(Problem => Problem.Description == desc);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " + MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Return the First instace of that Email found in the database
            return selectedProblem.FirstOrDefault();
        }

        public List<Problem> GetAll()
        {
            // Create the List to hold all of the Employee objects
            List<Problem> allProblems = new List<Problem>();

            try
            {
                // Gets all of the Employees and adds the objects to the List
                allProblems = repo.GetAll().ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Problem in " + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }

            // Return the List populated with the Employee objects
            return allProblems;
        }
    }
}
