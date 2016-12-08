using ParkInspect.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkInspect.Services
{
    public class AbsenceService
    {

        private readonly IRepository _context;
        public AbsenceService(IRepository context)
        {
            _context = context;
        }

        public IEnumerable<Absence> GetAllAbsences()
        {
            return _context.GetAll<Absence>();

        }

        public IEnumerable<Employee> GetAllEmployees()
        {
            return _context.GetAll<Employee>();
        }

        public void InsertAbsence(Absence a)
        {
            _context.Create(a);
            _context.Save();
        }

        public void DeleteAbsence(Absence a)
        {
            _context.Delete(a);
            _context.Save();
        }

    }
}
