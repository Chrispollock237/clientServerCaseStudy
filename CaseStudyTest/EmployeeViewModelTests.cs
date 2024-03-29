﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HelpdeskViewModels;
using System.Collections.Generic;

namespace CaseStudyTest
{
    [TestClass]
    public class EmployeeViewModelTests
    {
            [TestMethod]
        public void EmployeeViewModelGetbyNameShouldPopulatePropertyFirstname()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Lastname = "Smartypants"; // looking for a existing student
            vm.GetByLastname();
            Assert.IsNotNull(vm.Firstname);
        }

        [TestMethod]
        public void EmployeeViewModelAddShouldReturnId()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Title = "Mr.";
            vm.Firstname = "Chris";
            vm.Lastname = "Pollock";
            vm.Email = "ts@abc.com";
            vm.Phoneno = "(555)555-5551";
            vm.DepartmentId = 100;
            vm.Add();
            Assert.IsTrue(vm.Id > 0);
        }

        [TestMethod]
        public void EmployeeViewModelGetAllShouldReturnAtLeastOneVM()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            List<EmployeeViewModel> allStudentVms = vm.GetAll();
            Assert.IsTrue(allStudentVms.Count > 0);
        }

        [TestMethod]
        public void EmployeeViewModelGetByIdShouldPopulatePropertyFirstname()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Lastname = "Pollock";
            vm.GetByLastname(); // retrieve Employee just added should populate Id
            vm.GetById();
            Assert.IsNotNull(vm.Firstname);
        }

        [TestMethod]

        public void EmployeeModelUpdateShouldReturnOkStatus()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Lastname = "Pollock";
            vm.GetByLastname(); // Employee just added
            vm.Email = (vm.Email.IndexOf(".ca") > 0) ? "ts@abc.com" : "ts@abc.ca";
            int EmployeesUpdated = vm.Update();
            Assert.IsTrue(EmployeesUpdated > 0);
        }

        [TestMethod]
        public void EmployeeViewModelUpdateTwiceShouldReturnNegativeTwo()
        {
            EmployeeViewModel vm1 = new EmployeeViewModel();
            EmployeeViewModel vm2 = new EmployeeViewModel();
            vm1.Lastname = "Pollock";
            vm2.Lastname = "Pollock";
            vm1.GetByLastname();
            vm2.GetByLastname();
            vm1.Email = (vm1.Email.IndexOf(".ca") > 0) ? "ts@abc.com" : "ts@abc.ca";
            if(vm1.Update() == 1) // update works first time
            {
                vm2.Email = (vm2.Email.IndexOf(".ca") > 0) ? "ts@abc.com" : "ts@abc.ca";
                Assert.IsTrue(vm2.Update() == -2); // -2 = stale
            }
            else
                Assert.Fail();
        }

        [TestMethod]
        public void EmployeeModelDeleteShouldReturnOne()
        {
            EmployeeViewModel vm = new EmployeeViewModel();
            vm.Lastname = "Pollock";
            vm.GetByLastname(); //Employee just added
            int employeesDeleted = vm.Delete();
            Assert.IsTrue(employeesDeleted == 1);
        }
        
    }
}
