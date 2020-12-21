using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Models;
using RestaurantManagement.Contracts.Settings;
using RestaurantManagement.Interfaces;
using RestaurantManagement.Interfaces.Repositories;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class StaffService : IStaffService
    {
        private readonly ILogicHandler _logicHandler;
        private readonly ConfigurationSettings _options;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Staff> _staffRepo;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _loggerManager;

        public StaffService(ILogicHandler logicHandler, IOptions<ConfigurationSettings> options,
            IUnitOfWork unitOfWork, IMapper mapper, ILoggerManager loggerManager)
        {
            _logicHandler = logicHandler;
            _options = options.Value;
            _unitOfWork = unitOfWork;
            _staffRepo = _unitOfWork.GetRepository<Staff>();
            _mapper = mapper;
            _loggerManager = loggerManager;
        }

        public List<Staff> GetInitialStaff()
        {
            var initialStuffFile = _options.InitialStaff;
            var fileParts = _logicHandler.FileReader(initialStuffFile);
            var staffList = new List<Staff>();

            foreach (var staff in fileParts.Select(subList => new Staff()
            {
                Id = int.Parse(subList[0]),
                UserName = subList[1],
                UserPassword = subList[2],
                PersonRoleId = int.Parse(subList[3]),
                StartDayOfEmployment = Convert.ToDateTime(subList[4]),
                EndDayOfEmployment = Convert.ToDateTime(subList[5])
            }).Where(staff => staffList.All(x => x.UserName != staff.UserName)))
            {
                staffList.Add(staff);
            }
            return staffList;
        }

        public async Task<List<Staff>> GetStaff()
        {
            return await _staffRepo.Get();
        }

        public async Task<List<Staff>> SearchStaff(string searchInput)
        {
            return await _staffRepo.Get(x => x.UserName.Contains(searchInput));
        }

        public async Task<List<Staff>> GetStaffMember(string member)
        {
            return await _staffRepo.Get(x => x.UserName == member);
        }

        public async Task<Staff> GetStaffMemberById(int id)
        {
            return await _staffRepo.GetFirstOrDefault(x => x.Id == id);
        }

        public async Task<bool> CheckIfStaffMemberExists(string userName)
        {
            var memberList = await _staffRepo.Get(x => x.UserName == userName);
            var memberAmount = memberList.Select(x => x.UserName).Count();
            if (memberAmount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckIfStaffMemberIdExists(int memberId)
        {
            var memberList = await _staffRepo.Get(x => x.Id == memberId);
            var memberAmount = memberList.Select(x => x.UserName).Count();
            if (memberAmount > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task InsertAdditionalStaffMember(string userName, string userPassword, 
            int personRoleId, DateTime startDay, DateTime endDay)
        {
            var personRole = await _unitOfWork.GetRepository<PersonRole>().Get(x => x.Id == personRoleId);
            var additionalStaffMember = new Staff
            {
                UserName = userName,
                UserPassword = userPassword,
                PersonRoleId = personRole.Select(x => x.Id).First(),
                StartDayOfEmployment = startDay,
                EndDayOfEmployment = endDay

            };
            await _staffRepo.Add(additionalStaffMember);
            await _unitOfWork.Commit();
        }

        // FOR API
        public async Task<StaffViewModel> CreateStaffMember(StaffCreateModel staffCreateEntity)
        {
            var staffEntity = _mapper.Map<Staff>(staffCreateEntity);
            await _staffRepo.Add(staffEntity);
            await _unitOfWork.Commit();
            _loggerManager.LogInfo($"CreateStaffMember(): New staff member " +
                $"'{staffEntity.UserName}' is" +
                $" successfully created!");
            return _mapper.Map<StaffViewModel>(staffEntity);
        }

        public async Task DeleteStaffMember(int id)
        {
            var staffEntity = await _staffRepo.GetFirstOrDefault(x => x.Id == id);
            if (staffEntity == null)
                _loggerManager.LogError($"DeleteStaffMember(): Staff member {id} was not found!");
            else
            {
                _staffRepo.Delete(staffEntity);
                await _unitOfWork.Commit();
                _loggerManager.LogInfo($"DeleteStaffMember(): Staff member " +
                    $"'{staffEntity.UserName}' was deleted!");
            }
        }

        public async Task UpdateStaffMember(int id, StaffUpdateModel staffEntity)
        {
            var staffMemberEntity = await _staffRepo.GetFirstOrDefault(x => x.Id == id);
            if (staffMemberEntity == null)
                _loggerManager.LogError($"UpdateStaffMember(): Staff member {id} was not found!");

            else if(await NameIsAlreadyInUse(staffEntity)){
                _loggerManager.LogWarn($"UpdateStaffMember(): Staff member {id} was not updated! " +
                    $"Same name already exists!");
            }
            else
            {
                await Update(staffMemberEntity, staffEntity);
            }
        }

        private async Task<bool> NameIsAlreadyInUse(StaffUpdateModel staffEntity)
        {
            var existingNameCheck = await _staffRepo.GetFirstOrDefault(x => x.UserName == staffEntity.UserName);
            if(existingNameCheck == null)
                return false;
            return true;
        }

        private async Task Update(Staff staffMemberEntity, StaffUpdateModel staffEntity)
        {
            staffMemberEntity.UserName = staffEntity.UserName;
            staffMemberEntity.UserPassword = staffEntity.UserPassword;
            staffMemberEntity.PersonRoleId = staffEntity.PersonRoleId;
            staffMemberEntity.EndDayOfEmployment = staffEntity.EndDayOfEmployment;
            await _unitOfWork.Commit();
            _loggerManager.LogInfo($"UpdateStaffMember.Update(): Staff member " +
                $"'{staffMemberEntity.Id}' was updated!");
        }



    }
}
