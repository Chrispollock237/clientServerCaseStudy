using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelpdeskDAL;
using HelpdeskViewModels;
using System.Reflection;

namespace HelpdeskViewModels
{
    public class ProblemViewModel
    {
        private ProblemModel _model;
        public string Description { get; set; }
        public int Id { get; set; }
        public ProblemViewModel()
        {
            _model = new ProblemModel();
        }

        public void GetByDescription()
        {
            try
            {
                // New Employee object that will be created with the Lastname
                Problem problem = _model.GetByDescription(Description);
                Description = problem.Description;
                Id = problem.Id;
                //Timer = Convert.ToBase64String(problem.Timer);

            }
            catch (Exception ex)
            {
                Description = "not found";
                Console.WriteLine("Problem in" + GetType().Name + " " +
                    MethodBase.GetCurrentMethod().Name + " " + ex.Message);
                throw ex;
            }
        }

        public List<ProblemViewModel> GetAll()
        {
            // Create a list to hold the ProblemViewModell objects
            List<ProblemViewModel> allVms = new List<ProblemViewModel>();
            try
            {
                // Create the List to hold the Employee objects. Uses the GetAll()
                // from the EmployeeModel class to populate this List
                List<Problem> allProblems = _model.GetAll();

                // For each Employee in the allEmployees List create a view model for them and 
                // then add them to the List of Employee objects
                foreach (Problem problem in allProblems)
                {
                    ProblemViewModel probVm = new ProblemViewModel();
                    probVm.Id = problem.Id;
                    probVm.Description = problem.Description;
                    //probVm.Timer = Convert.ToBase64String(problem.Timer);
                    allVms.Add(probVm);
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
    }
}
