using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Enums;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Interfaces.Services;
using RestaurantManagement.WebApp.Models;

namespace RestaurantManagement.WebApp.Controllers
{
    public class StaffController : Controller
    {
        private readonly IUserLogService _userLogService;
        private readonly IStaffService _staffService;

        public StaffController(IUserLogService userLogService, IStaffService staffService)
        {
            _userLogService = userLogService;
            _staffService = staffService;
        }

        public async Task<IActionResult> Index(int? pageIndex, string searchInput, int pageSize = 2)
        {
            try
            {
                List<Staff> staffList;

                if (!string.IsNullOrEmpty(searchInput))
                {
                    staffList = await _staffService.SearchStaff(searchInput);
                    pageSize = staffList.Count;
                }
                else
                {
                    staffList = await _staffService.GetStaff();
                }
                // Konstanta index
                // Keisti approach - 1) Lazy loading better or 2) Delegate
                var paginatedList = PaginatedList<Staff>.Create(staffList, pageIndex ?? 1, pageSize);

                return View(paginatedList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public async Task<IActionResult> StaffMemberDetails(string member)
        {
            try
            {
                if (member == null)
                    throw new Exception("Error! At least one word must be entered.");

                var memberList = await _staffService.GetStaffMember(member);
                return View(memberList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(member);
            }
        }
        public IActionResult StaffAddition()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StaffAddition([Bind("UserName, UserPassword, PersonRoleId, StartDayOfEmployment, EndDayOfEmployment")] Staff staffEntity)
        {
            if (ModelState.IsValid)
            {
                var userName = staffEntity.UserName;
                var userPassword = staffEntity.UserPassword;
                var personRoleId = staffEntity.PersonRoleId;
                var startDay = Convert.ToDateTime(staffEntity.StartDayOfEmployment);
                var endDay = Convert.ToDateTime(staffEntity.StartDayOfEmployment);

                var exists = await _staffService.CheckIfStaffMemberExists(userName);

                if (exists)
                {
                    ModelState.AddModelError(string.Empty, "User with same name already exists!");
                    return View();
                }
                else
                {
                    await _staffService.InsertAdditionalStaffMember(userName, userPassword, personRoleId, startDay, endDay);
                    _userLogService.InsertUserLog(UserAction.Add_Staff, 1);
                    ViewBag.Message = "New word added successfully!";
                    return View();
                }
            }
            return View();
        }

        public IActionResult StaffRemoval()
        {
            return View();
        }

        [HttpPost, ActionName("StaffRemoval")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovalConfirmed(int id)
        {
            if (ModelState.IsValid)
            {
                var exists = await _staffService.CheckIfStaffMemberIdExists(id);
                if (!exists)
                {
                    ModelState.AddModelError(string.Empty, "Specified member does not exist!");
                    return View();
                }
                else
                {
                    await _staffService.DeleteStaffMember(id);
                    _userLogService.InsertUserLog(UserAction.Remove_Staff, 1);
                    ViewBag.Message = "Selected member was deleted!";
                    return View();
                }
            }
            return View();
        }

        public IActionResult StaffUpdate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StaffUpdate(int id, 
            [Bind("UserName, UserPassword, PersonRoleId, EndDayOfEmployment")] StaffUpdateModel staffEntity)
        {
            if (ModelState.IsValid)
            {
                var exists = await _staffService.CheckIfStaffMemberIdExists(id);
                var userId = 1;

                if (!exists)
                {
                    ModelState.AddModelError(string.Empty, "Specified staff member does not exist!");
                    return View();
                }
                else
                {
                    await _staffService.UpdateStaffMember(id, staffEntity);
                    _userLogService.InsertUserLog(UserAction.Update_Staff, userId);
                    ViewBag.Message = "If new user name was unique, member was updated!" +
                        "If not, check the logs.";
                    return View();
                }
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
