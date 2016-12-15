using System.Collections.Generic;
using System.Linq;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    public class InspectionService
    {
        private readonly IRepository _context;

        public InspectionService(IRepository context)
        {
            _context = context;
        }
        public void AddInspection(Inspection i)
        {
            _context.Create(i);
            _context.Save();
        }

        public void UpdateInspection(Inspection i)
        {
            _context.Update(i);
            _context.Save();
        }

        public IEnumerable<Inspection> GetAllInspections()
        {
            return _context.GetAll<Inspection>();
        }

        public Inspection GetInspectionByID(int id)
        {
            return _context.Get<Inspection>().Where(i => i.id == id).First();
        }
    }
}
