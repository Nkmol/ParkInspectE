using ParkInspect.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkInspect.Services
{
    public class EmployeeService
    {
        private readonly IRepository _context;

        public EmployeeService(IRepository context)
        {
            _context = context;
        }

        public IEnumerable<Employee> GetEmployee(string email, string password)
        {
            return _context.Get<Employee>()
                .Where(k => k.email == email && k.password == password);
        }
    }
}
