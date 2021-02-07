using Microsoft.Extensions.Options;
using RestaurantManagement.Contracts.Entities;
using RestaurantManagement.Contracts.Settings;
using RestaurantManagement.Interfaces;
using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class UnitOfMeasurementService : IUnitOfMeasurementService
    {
        private readonly ILogicHandler _logicHandler;
        private readonly ConfigurationSettings _options;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UnitOfMeasurement> _unitOfMeasurementRepo;

        public UnitOfMeasurementService(ILogicHandler logicHandler, IOptions<ConfigurationSettings> options, IUnitOfWork unitOfWork)
        {
            _logicHandler = logicHandler;
            _options = options.Value;
            _unitOfMeasurementRepo = unitOfWork.GetRepository<UnitOfMeasurement>();
        }

        public List<UnitOfMeasurement> GetInitialUnitsOfMeasurement()
        {
            var initialUnitsFile = _options.InitialUnits;
            var fileParts = _logicHandler.FileReader(initialUnitsFile);
            var unitsList = new List<UnitOfMeasurement>();

            foreach (var unit in fileParts.Select(subList => new UnitOfMeasurement()
            {
                Id = int.Parse(subList[0]),
                UnitName = subList[1],
                UnitDescription = subList[2]
            }).Where(unit => unitsList.All(x => x.UnitName != unit.UnitName)))
            {
                unitsList.Add(unit);
            }
            return unitsList;
        }
    }
}
