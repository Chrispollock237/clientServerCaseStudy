using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskDAL;
using System.Collections.Generic;
using HelpdeskViewModels;

namespace CaseStudyTest
{
    [TestClass]
    public class CallViewModelTests
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void ComprehensiveVMTests()
        {
            CallViewModel cvm = new CallViewModel();
            EmployeeViewModel evm = new EmployeeViewModel();
            ProblemViewModel pvm = new ProblemViewModel();
            cvm.DateOpened = DateTime.Now;
            cvm.DateClosed = null;
            cvm.OpenStatus = true;
            evm.Lastname = "Pollock";
            evm.GetByLastname();
            cvm.EmployeeId = evm.Id;
            evm.Lastname = "Burner";
            evm.GetByLastname();
            cvm.TechId = evm.Id;
            pvm.Description = "Memory Upgrade";
            pvm.GetByDescription();
            cvm.ProblemId = pvm.Id;
            cvm.Notes = "Chris has bad RAM, Burner to fix it";
            cvm.Add();
            Console.WriteLine("New Call Generated - Id = " + cvm.Id);
            int id = cvm.Id; // need Id for delete later
            cvm.GetById();
            cvm.Notes += "\n Ordered new RAM!";

            if(cvm.Update() == 1)
            {
                Console.WriteLine("Call was updated " + cvm.Notes);
            }
            else
            {
                Console.WriteLine("Call was NOT updated!");
            }

            cvm.Notes = "Another change to comments that should not work.";
            if (cvm.Update() == -2)
            {
                Console.WriteLine("Call was NOT updated, data was stale.");
            }

            cvm = new CallViewModel(); // need to reset because of Concurrency
            cvm.Id = id;
            cvm.GetById();

            if(cvm.Delete() == 1)
            {
                Console.WriteLine("Call was deleted!");
            }
            else
            {
                Console.WriteLine("Call was NOT deleted!");
            }

            cvm.GetById(); // should throw expected exception
        }
    }
}
