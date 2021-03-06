using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NUnit.Framework;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Interfaces.Services;
using RestaurantManagement.WebApp.Controllers;
using RestaurantManagement.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantManagement.Tests
{
    public class StaffControllerTests
    {
        IUserLogService _userLogServiceMock;
        IStaffService _staffServiceMock;
        StaffController _staffController;

        [SetUp]
        public void Setup()
        {
            _staffServiceMock = Substitute.For<IStaffService>();
            _userLogServiceMock = Substitute.For<IUserLogService>();
            _staffController = new StaffController(_userLogServiceMock, _staffServiceMock);
        }

        [Test]
        public async Task TestIndex_IfSearchIsPerformed()
        {
            var testStaffEntity = new Staff()
            {
                UserName = "Kipras Narbutas",
                UserPassword = "gkrc71658*",
                PersonRoleId = 3,
                StartDayOfEmployment = new DateTime(2020,11,16),
                EndDayOfEmployment = new DateTime(2025,08,16)
            };

            var testStaffList = new List<Staff>() { testStaffEntity };

            _staffServiceMock.SearchStaff(Arg.Any<string>()).Returns(testStaffList);

            var result = await _staffController.Index(1, testStaffEntity.UserName) as ViewResult;
            var model = result.ViewData.Model as PaginatedList<Staff>;

            await _staffServiceMock.Received().SearchStaff(Arg.Any<string>());

            Assert.AreEqual(testStaffList, model);
        }

        [Test]
        public async Task TestIndex_IfSearchIsNotPerformed()
        {
            var searchMember = "";
            var testStaffEntity = new Staff()
            {
                UserName = "Edgaras Legus",
                UserPassword = "124564*",
                PersonRoleId = 4,
                StartDayOfEmployment = new DateTime(2020, 10, 26),
                EndDayOfEmployment = new DateTime(2025, 10, 26)
            };
            var testStaffList = new List<Staff>() { testStaffEntity };

            _staffServiceMock.GetStaff().Returns(testStaffList);

            var result = await _staffController.Index(1, searchMember) as ViewResult;
            var model = result.ViewData.Model as PaginatedList<Staff>;

            await _staffServiceMock.Received().GetStaff();

            Assert.AreEqual(testStaffList, model);

        }

        [Test]
        public async Task TestStaffMemberDetails_IfReturnedViewIsCorrect()
        {
            var testStaffEntity = new Staff()
            {
                UserName = "Edgaras Legus",
                UserPassword = "124564*",
                PersonRoleId = 4,
                StartDayOfEmployment = new DateTime(2020, 10, 26),
                EndDayOfEmployment = new DateTime(2025, 10, 26)
            };

            var testStaffList = new List<Staff>() { testStaffEntity };

            _staffServiceMock.GetStaffMember(testStaffEntity.UserName).Returns(testStaffList);

            var result = await _staffController.StaffMemberDetails(testStaffEntity.UserName) as ViewResult;
            var model = result.ViewData.Model as Staff;

            await _staffServiceMock.Received().GetStaffMember(testStaffEntity.UserName);

            Assert.AreEqual(testStaffList, model);
        }

        [Test]
        public async Task Test_StaffAdditionValidationMessage_IfModelIsNotValid_WhenPasswordIsTooShort()
        {

            var testStaffEntity = new Staff()
            {
                UserName = "Edgaras Legus",
                UserPassword = "fd4",
                PersonRoleId = 4,
                StartDayOfEmployment = new DateTime(2020, 10, 26),
                EndDayOfEmployment = new DateTime(2025, 10, 26)
            };
            var error = "Must be between 5 and 50 characters";
            _staffController.ModelState.AddModelError("UserPassword", error);

            var viewResult = await _staffController.StaffAddition(testStaffEntity) as ViewResult;

            var errorList = new List<string>();
            foreach (var item in viewResult.ViewData.ModelState.Values)
            {
                foreach (var errormessage in item.Errors)
                {
                    errorList.Add(errormessage.ErrorMessage);
                }
            }
            var myMessage = string.Join(",", errorList.ToArray());

            Assert.AreEqual(error, myMessage);

        }

        [Test]
        public async Task Test_StaffAddition_ErrorMessageComparison_IfUserNameAlreadyExists()
        {
            var testStaffEntity = new Staff()
            {
                UserName = "Kestutis Karys",
                UserPassword = "fd4dfsdf",
                PersonRoleId = 4,
                StartDayOfEmployment = new DateTime(2020, 10, 26),
                EndDayOfEmployment = new DateTime(2025, 10, 26)
            };

            var alertMessage = "User with same name already exists!";

            _staffServiceMock.CheckIfStaffMemberExists(testStaffEntity.UserName).Returns(true);

            var viewResult = await _staffController.StaffAddition(testStaffEntity) as ViewResult;

            var errorList = new List<string>();
            foreach (var item in viewResult.ViewData.ModelState.Values)
            {
                foreach (var errormessage in item.Errors)
                {
                    errorList.Add(errormessage.ErrorMessage);
                }
            }
            var actualMessage = string.Join(",", errorList.ToArray());

            await _staffServiceMock.Received().CheckIfStaffMemberExists(testStaffEntity.UserName);

            Assert.AreEqual(alertMessage, actualMessage);
        }

        [Test]
        public async Task Test_StaffAddition_ViewBagContent_IfStaffMemberIsNew()
        {
            var testStaffEntity = new Staff()
            {
                UserName = "Kestutis Karys",
                UserPassword = "fd4dfsdf",
                PersonRoleId = 4,
                StartDayOfEmployment = new DateTime(2020, 10, 26),
                EndDayOfEmployment = new DateTime(2025, 10, 26)
            };

            _staffServiceMock.CheckIfStaffMemberExists(testStaffEntity.UserName).Returns(false);

            var viewResult = await _staffController.StaffAddition(testStaffEntity) as ViewResult;
            var message = "New word added successfully!";

            await _staffServiceMock.Received().CheckIfStaffMemberExists(testStaffEntity.UserName);
            Assert.AreEqual(message, _staffController.ViewBag.Message);

        }

    }
}