using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RestaurantManagement.Interfaces.Services;
using RestaurantManagement.Data;
using RestaurantManagement.WebApp.Models;

namespace RestaurantManagement.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IDataLoader _dataLoader;
        private readonly RestaurantManagementCodeFirstContext _context;

        public HomeController(ILogger<HomeController> logger, IDataLoader dataLoader, RestaurantManagementCodeFirstContext context)
        {
            _logger = logger;
            _dataLoader = dataLoader;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Try catch nereikia
            try
            {
                await _dataLoader.LoadInitialData();
                //await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
