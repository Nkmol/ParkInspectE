using ParkInspect.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkInspect.Services
{
    public class AbsenceService : DataService
    {
        private readonly IRepository _context;
        public AbsenceService(IRepository context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<Absence> GetAllAbsences()
        {
            return _context.GetAll<Absence>();

        }


    }
}
