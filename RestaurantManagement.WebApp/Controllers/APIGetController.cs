using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Interfaces.Repositories;

namespace RestaurantManagement.WebApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class APIGetController : ControllerBase
    {
        private readonly IStaffRepo _staffRepo;

        public APIGetController(IStaffRepo staffRepo)
        {
            _staffRepo = staffRepo;
        }

        [HttpGet("staff/{id}")]
        public async Task<IActionResult> GetApiStaffMember(string id)
        {

            try
            {
                if (string.IsNullOrEmpty(id))
                    throw new Exception("Error! At least one word must be entered.");

                var staffMember = await _staffRepo.GetStaffMember(id);

                return Ok(staffMember);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
