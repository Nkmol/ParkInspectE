using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    class InspectionService : DataService
    {
        private readonly IRepository _context;

        public InspectionService(IRepository context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<Inspection> GetInspectionWithId(int id)
        {
            return _context.GetAll<Inspection>(null, c => c.Employees, k => k.Inspection1)
                .Where(k => k.id == id);
        }
    }
}
