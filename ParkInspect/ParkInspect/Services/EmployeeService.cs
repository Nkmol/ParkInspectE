﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    public class EmployeeService
    {
        private readonly IRepository _context;

        public EmployeeService(IRepository context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.GetAll<Employee>();
        }

        public void InsertEntity(Employee e)
        {
            _context.Create(e);
            _context.Save();
        }

        public void UpdateEntity(Employee e)
        {
            _context.Update(e);
            _context.Save();
        }

        public void DeleteEntity(Employee e)
        {
            if(_context.Get<Employee>(x => x.id == e.id).Any())
            {
                _context.Delete(e);
                _context.Save();
            }
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return _context.GetAll<Role>();
        }

        public IEnumerable<Employee_Status> GetAllStatusses()
        {
            return _context.GetAll<Employee_Status>();
        }


    }
}