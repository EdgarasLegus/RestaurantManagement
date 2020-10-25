using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Contracts.Entities;

namespace RestaurantManagement.WebApp.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateOrder()
        {
            return View();
        }

        //POST - create new order

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> CreateOrder([Bind("OrderName")] Order staffEntity)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userName = staffEntity.UserName;
        //        var userPassword = staffEntity.UserPassword;
        //        var personRoleId = staffEntity.PersonRoleId;
        //        var startDay = Convert.ToDateTime(staffEntity.StartDayOfEmployment);
        //        var endDay = Convert.ToDateTime(staffEntity.StartDayOfEmployment);

        //        //var ip = _eflogic.GetIP();
        //        var userId = 1;

        //        var exists = _staffRepo.CheckIfStaffMemberExists(userName);

        //        if (exists)
        //        {
        //            ModelState.AddModelError(string.Empty, "User with same name already exists!");
        //            return View();
        //        }
        //        else
        //        {
        //            await _staffRepo.InsertAdditionalStaffMember(userName, userPassword, personRoleId, startDay, endDay);
        //            _userLogRepo.InsertUserLog(UserAction.Add_Staff, userId);
        //            ViewBag.Message = "New word added successfully!";
        //            return View();
        //        }
        //    }
        //    return View();
        //}
    }
}
