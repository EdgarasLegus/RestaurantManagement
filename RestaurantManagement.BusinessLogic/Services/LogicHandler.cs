using RestaurantManagement.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RestaurantManagement.BusinessLogic.Services
{
    public class LogicHandler : ILogicHandler
    {
        public bool BooleanConverter(string value)
        {
            return value == "1";
        }

        public List<List<string>> FileReader(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new Exception($"Data file {fileName} does not exist!");
            }

            var partList = new List<List<string>>();
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',').Select(p => p.Trim()).ToList();
                    partList.Add(parts);
                }
            }
            return partList;
        }
    }
}
