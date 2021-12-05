﻿using Dapper.Example.Models;
using System.Collections.Generic;

namespace Dapper.Example.Repository
{
    public interface IBonusRepository
    {
        List<Employee> GetEmployeesWithCompany(int id);
    }
}