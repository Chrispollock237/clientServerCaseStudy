using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskDAL;
using System.Collections.Generic;

namespace CaseStudyTest
{
    [TestClass]
    public class EmployeeModelTests
    {
        [TestMethod]
        public void EmployeeModelGetbyEmailShouldReturnEmployee()
        {
            EmployeeModel model = new EmployeeModel();
            Employee someEmployee = model.GetByEmail("bs@abc.com");
            Assert.IsNotNull(someEmployee);
        }

        [TestMethod]
        public void EmployeeModelGetbyEmailShouldNotReturnEmployee()
        {
            EmployeeModel model = new EmployeeModel();
            Employee someEmployee = model.GetByEmail("bs@abq.com");
            Assert.IsNull(someEmployee);
        }

        [TestMethod]
        public void EmployeeModelModelGetAllShouldReturnList()
        {
            EmployeeModel model = new EmployeeModel();
            List<Employee> allEmployees = model.GetAll(); ;
            Assert.IsTrue(allEmployees.Count > 0);
        }

        [TestMethod]
        public void EmployeeModelGetAllShouldReturnNewId()
        {
            EmployeeModel model = new EmployeeModel();
            Employee newEmployee = new Employee();
            newEmployee.Title = "Mr.";
            newEmployee.FirstName = "Test";
            newEmployee.LastName = "Employee";
            newEmployee.Email = "ts@abc.com";
            newEmployee.PhoneNo = "(555)555-5551";
            newEmployee.DepartmentId = 100;
            int newId = model.Add(newEmployee);
            Assert.IsTrue(newId > 0);
        }

        [TestMethod]
        public void EmployeeModelGetbyIdShouldReturnEmployee()
        {
            EmployeeModel model = new EmployeeModel();
            Employee someEmployee = model.GetByLastname("Employee"); // retrieve Employee we just added
            Employee anotherEmployee = model.GetById(someEmployee.Id); // this is for the actual test
            Assert.IsNotNull(anotherEmployee);
        }

        [TestMethod]
        public void EmployeeModelUpdateShouldReturnOne()
        {
            EmployeeModel model = new EmployeeModel();
            Employee updateEmployee = model.GetByLastname("Employee"); // retrieve Employee we just added
            updateEmployee.Email = "ts.xyz.com";
            UpdateStatus EmployeesUpdated = model.UpdateForConcurrency(updateEmployee);
            Assert.IsTrue(EmployeesUpdated == UpdateStatus.Ok);
        }

        [TestMethod]
        public void EmployeeModelUpdateTwiceShouldReturnStaleStatus()
        {
            EmployeeModel model1 = new EmployeeModel();
            EmployeeModel model2 = new EmployeeModel();
            Employee updateEmployee1 = model1.GetByLastname("Employee"); // should already exist
            Employee updateEmployee2 = model2.GetByLastname("Employee"); // should already exist
            updateEmployee1.Email = (updateEmployee1.Email.IndexOf(".ca") > 0) ? "ts@abc.com" : "ts@abc.ca";
            if (model1.UpdateForConcurrency(updateEmployee1) == UpdateStatus.Ok)
            {
                updateEmployee2.Email = (updateEmployee2.Email.IndexOf(".ca") > 0) ? "ts@abc.com" : "ts@abc.ca";
                Assert.IsTrue(model2.UpdateForConcurrency(updateEmployee2) == UpdateStatus.Stale);
            }
            else
                Assert.Fail();
        }

        [TestMethod]
        public void EmployeeModelDeleteShouldReturnOne()
        {
            EmployeeModel model = new EmployeeModel();
            Employee deleteEmployee = model.GetByLastname("Employee"); // Employee we just added
            int EmployeeDeleted = model.Delete(deleteEmployee.Id);
            Assert.IsTrue(EmployeeDeleted == 1);
        }

        [TestMethod]
        public void LoadPicsShouldReturnTrue()
        {
            DALUtil util = new DALUtil();
            Assert.IsTrue(util.AddEmployeePicsToDb());
        }

    }
}
