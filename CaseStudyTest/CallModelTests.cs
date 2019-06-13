using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskDAL;
using System.Collections.Generic;

namespace CaseStudyTest
{
    [TestClass]
    public class CallModelTests
    {
        [TestMethod]
        public void ComprehensiveModelTestsShouldReturnTrue()
        {
            CallModel cmodel = new CallModel();
            EmployeeModel emodel = new EmployeeModel();
            ProblemModel pmodel = new ProblemModel();
            Call call = new Call();
            call.DateOpened = DateTime.Now;
            call.DateClosed = null;
            call.OpenStatus = true;
            call.EmployeeId = emodel.GetByLastname("Pollock").Id;
            call.TechId = emodel.GetByLastname("Burner").Id;
            call.ProblemId = pmodel.GetByDescription("Hard Drive Failure").Id;
            call.Notes = "Chris' drive is shot, Burner to fix it.";
            int newCallId = cmodel.Add(call);
            Console.WriteLine("New Call Generated - Id = " + newCallId);
            call = cmodel.GetById(newCallId);
            byte[] oldtimer = call.Timer;
            Console.WriteLine("New Call Retrieved");
            call.Notes += "\n Ordered new RAM!";

            if (cmodel.Update(call) == UpdateStatus.Ok)
            {
                Console.WriteLine("Call was updated " + call.Notes);
            }
            else
            {
                Console.WriteLine("Call was NOT updated!");
            }

            call.Timer = oldtimer;
            if(cmodel.Delete(newCallId) == 1)
            {
                Console.WriteLine("Call was deleted!");
            }
            else
            {
                Console.WriteLine("Call was NOT deleted");
            }

            Assert.IsNull(cmodel.GetById(newCallId));
        }
    }
}
