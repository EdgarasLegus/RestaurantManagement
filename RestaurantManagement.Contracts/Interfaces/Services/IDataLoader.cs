﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantManagement.Contracts.Interfaces.Services
{
    public interface IDataLoader
    {
        Task LoadInitialData();
    }
}
