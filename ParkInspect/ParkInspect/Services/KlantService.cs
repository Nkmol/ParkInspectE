using System.Collections.Generic;
using System.Linq;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    public class KlantService
    {
        private readonly IRepository _context;

        public KlantService(IRepository context)
        {
            _context = context;
        }

//        public IEnumerable<Klant> GetKlantsWithName(string name)
//        {
//            return _context.GetAll<Klant>().Where(k => k.naam == name);
//        }
    }
}
