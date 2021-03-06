﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Interfaces.Services;

namespace RestaurantManagement.WebApp.Controllers
{
    [Route("api/v1/staff")]
    [ApiController]
    public class APIStaffController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;
        private readonly IStaffService _staffService;

        public APIStaffController(IMapper mapper, ILoggerManager loggerManager,
            IStaffService staffService)
        {
            _mapper = mapper;
            _loggerManager = loggerManager;
            _staffService = staffService;
        }

        [HttpGet]
        public async Task<IActionResult> GetStaff()
        {
            // Naming
            // Try catch - out, bad request ideti i if tiesiog.
            // Id , externalID
            try
            {
                var staff = await _staffService.GetStaff();
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
                var staffMember = await _staffService.GetStaffMemberById(id);

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
                var createdMember = await _staffService.CreateStaffMember(staffMember);
                return CreatedAtRoute("StaffMemberById", new { id = createdMember.Id }, createdMember);
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"CreateStaffMember() method execution failed: {ex.Message}");
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

                await _staffService.UpdateStaffMember(id, staffMember);

                return NoContent();
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"UpdateStaffMember() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaffMember(int id)
        {
            try
            {
                await _staffService.DeleteStaffMember(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _loggerManager.LogError($"DeleteStaffMember() method execution failed: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
