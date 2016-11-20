using System;
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
    }
}
