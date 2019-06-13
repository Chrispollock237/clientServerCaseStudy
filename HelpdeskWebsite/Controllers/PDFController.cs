using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HelpdeskWebsite;

namespace HelpdeskWebsite.Controllers
{
    public class PDFController : ApiController
    {
        [Route("api/employeereport")]
        public IHttpActionResult GetEmployeeReport()
        {
            try
            {
                Reports.EmployeeReport empRep = new Reports.EmployeeReport();
                empRep.doIt();
                return Ok("Report Generated");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error " + ex.Message);
                return BadRequest("Retrieve failed - Couldn't generate sample report");
            }
        }

        [Route("api/callreport")]
        public IHttpActionResult GetCallReport()
        {
            try
            {
                Reports.CallReport callRep = new Reports.CallReport();
                callRep.doIt();
                return Ok("Report Generated");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error " + ex.Message);
                return BadRequest("Retrieve failed - Couldn't generate sample report");
            }
        }
    }
}
