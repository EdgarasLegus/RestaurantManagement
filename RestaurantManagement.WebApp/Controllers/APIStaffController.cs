using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLog;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Interfaces.Repositories;
using RestaurantManagement.Interfaces.Services;

namespace RestaurantManagement.WebApp.Controllers
{
    [Route("api/v1/staff")]
    [ApiController]
    public class APIStaffController : ControllerBase
    {
        private readonly IStaffRepo _staffRepo;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public APIStaffController(IStaffRepo staffRepo, IMapper mapper, ILoggerManager loggerManager)
        {
            _staffRepo = staffRepo;
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetStaff()
        {
            // Naming
            // Try catch - out, bad request ideti i if tiesiog.
            // Id , externalID
            try
            {
                var staff = await _staffRepo.GetStaff();
                _loggerManager.LogInfo($"GetStaff() method returned Staff list.");
                var staffResult = _mapper.Map<IEnumerable<StaffViewModel>>(staff);
                return Ok(staffResult);
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"GetStaff() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}", Name = "StaffMemberById")]
        public async Task<IActionResult> GetStaffMemberById(int id)
        {
            try
            {
                var staffMember = await _staffRepo.GetStaffMemberById(id);

                if(staffMember == null)
                {
                    _loggerManager.LogError($"Staff Member with id: {id} is not found in database.");
                    return NotFound();
                }
                else
                {
                    _loggerManager.LogInfo($"GetStaffMemberById() returned staff member with id: {id}");
                    var staffMemberResult = _mapper.Map<StaffViewModel>(staffMember);
                    return Ok(staffMemberResult);
                }
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"GetStaffMemberById() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateStaffMember([FromBody]StaffCreateModel staffMember)
        {
            try
            {
                if(staffMember == null)
                {
                    _loggerManager.LogError($"");
                    return BadRequest("Staff member is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid staff member model object");
                }

                var staffMemberEntity = _mapper.Map<Staff>(staffMember);
                await _staffRepo.CreateStaffMember(staffMemberEntity);

                var createdMember = _mapper.Map<StaffViewModel>(staffMemberEntity);

                return CreatedAtRoute("StaffMemberById", new { id = createdMember.Id }, createdMember);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStaffMember(int id, [FromBody] StaffUpdateModel staffMember)
        {
            try
            {
                if (staffMember == null)
                {
                    return BadRequest("Staff member is null");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest("Invalid staff member model object");
                }

                await _staffRepo.UpdateStaffMemberInfo(id, staffMember);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaffMember(int id)
        {
            try
            {
                var staffMember = await _staffRepo.GetStaffMemberById(id);

                if (staffMember == null)
                {
                    return NotFound();
                }

                await _staffRepo.DeleteStaffMember(staffMember);

                return NoContent();

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
