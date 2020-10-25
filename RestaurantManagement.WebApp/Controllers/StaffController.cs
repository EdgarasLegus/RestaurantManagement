using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Enums;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Interfaces.Repositories;
using RestaurantManagement.WebApp.Models;

namespace RestaurantManagement.WebApp.Controllers
{
    public class StaffController : Controller
    {
        private readonly IStaffRepo _staffRepo;
        private readonly IUserLogRepo _userLogRepo;

        public StaffController(IStaffRepo staffRepo, IUserLogRepo userLogRepo)
        {
            _staffRepo = staffRepo;
            _userLogRepo = userLogRepo;
        }

        public async Task<IActionResult> Index(int? pageIndex, string searchInput, int pageSize = 2)
        {
            try
            {
                List<Staff> staffList;

                if (!string.IsNullOrEmpty(searchInput))
                {
                    staffList = await _staffRepo.SearchStaff(searchInput);
                    pageSize = staffList.Count;
                }
                else
                {
                    staffList = await _staffRepo.GetStaff();
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

        public async Task<IActionResult> StaffMemberDetails(string id)
        {
            try
            {
                if (id == null)
                    throw new Exception("Error! At least one word must be entered.");

                var memberList = await _staffRepo.GetStaffMember(id);
                //Staff member = memberList.FirstOrDefault(a => a.UserName);


                return View(memberList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(id);
            }
        }

        // GET - get form of word addition

        public IActionResult StaffAddition()
        {
            return View();
        }

        //POST - add word to dictionary

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

                //var ip = _eflogic.GetIP();
                var userId = 1;

                var exists = _staffRepo.CheckIfStaffMemberExists(userName);

                if (exists)
                {
                    ModelState.AddModelError(string.Empty, "User with same name already exists!");
                    return View();
                }
                else
                {
                    await _staffRepo.InsertAdditionalStaffMember(userName, userPassword, personRoleId, startDay, endDay);
                    _userLogRepo.InsertUserLog(UserAction.Add_Staff, userId);
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
        public async Task<IActionResult> RemovalConfirmed(string staffMemberName)
        {
            if (ModelState.IsValid)
            {
                //var ip = _eflogic.GetIP();
                //var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                var exists = _staffRepo.CheckIfStaffMemberExists(staffMemberName);
                var userId = 1;

                if (!exists)
                {
                    ModelState.AddModelError(string.Empty, "Specified member does not exist!");
                    return View();
                }
                else
                {
                    await _staffRepo.RemoveStaffMember(staffMemberName);
                    _userLogRepo.InsertUserLog(UserAction.Remove_Staff, userId);
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
        public IActionResult StaffUpdate(int updatableStaffMember, [Bind("UserName, UserPassword, PersonRoleId, EndDayOfEmployment")] StaffUpdateModel staffEntity)
        {
            if (ModelState.IsValid)
            {
                //var ip = _eflogic.GetIP();
                //var ip = HttpContext.Connection.RemoteIpAddress.ToString();
                var exists = _staffRepo.CheckIfStaffMemberIdExists(updatableStaffMember);
                var userId = 1;

                if (!exists)
                {
                    ModelState.AddModelError(string.Empty, "Specified staff member does not exist!");
                    return View();
                }
                else
                {
                    var updateCheck = _staffRepo.UpdateStaffMember(updatableStaffMember, staffEntity);
                    if (updateCheck == true)
                    {
                        ModelState.AddModelError(string.Empty, "This staff member cannot be updated with your option, same member already exists!");
                        return View();
                    }
                    else
                    {
                        _userLogRepo.InsertUserLog(UserAction.Update_Staff, userId);
                        ViewBag.Message = "Selected member was updated!";
                        return View();
                    }
                }
            }
            return View();
        }

        //public async Task<IActionResult> StaffEdit(int? id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (id == null)
        //        {
        //            ModelState.AddModelError(string.Empty, "Specified staff member does not exist!");
        //            return View();
        //        }

        //        var staffMemberId = await _staffRepo.GetStaffMemberId(id);

        //        if (staffMemberId == null)
        //        {
        //            ModelState.AddModelError(string.Empty, "Specified staff member does not exist!");
        //            return View();
        //        }
        //        return View(staffMemberId);
        //    }
        //    return View();
        //}

        //[HttpPost, ActionName("StaffEdit")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> StaffEdition(int? id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (id == null)
        //        {
        //            ModelState.AddModelError(string.Empty, "Specified staff member does not exist!");
        //            return View();
        //        }

        //        var staffMemberId = await _staffRepo.GetStaffMemberId(id);

        //        if (await TryUpdateModelAsync<Staff>(staffMemberId, "", 
        //            x => x.UserName, x => x.UserPassword, x => x.PersonRoleId, x => x.EndDayOfEmployment))
        //        {
        //            await _staffRepo.SaveStaffMemberEdition();
        //        }
        //        return View(staffMemberId);
        //    }
        //    return View();
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
