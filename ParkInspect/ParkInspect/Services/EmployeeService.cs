using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;
using System.Collections;

namespace ParkInspect.Services
{
    public class EmployeeService : DataService
    {
        public EmployeeService(IRepository context) : base(context)
        {
        }

        public Employee Get(int id)
        {
            return Context.Get<Employee>().Where(x => x.id == id).FirstOrDefault();
        }

        public bool Login(string email, string password)
        {
            return Context.Get<Employee>()
                       .Where(k => k.email == email && k.password == password)
                       .Count() != 0;
        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return Context.GetAll<Employee>();
        }

        public void DeleteEntity(Employee e)
        {
            if (Context.Get<Employee>(x => x.id == e.id).Any())
            {
                Delete(e);
            }
        }

        public IEnumerable<Role> GetAllRoles()
        {
            return Context.GetAll<Role>();
        }

        public Employee GetEmployee(string resultUsername, string resultPassword)
        {
            return
                Context.GetAll<Employee>()
                    .FirstOrDefault(k => k.email == resultUsername && k.password == resultPassword);

        }
    }
}
