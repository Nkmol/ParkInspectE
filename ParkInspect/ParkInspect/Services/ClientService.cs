using System.Collections.Generic;
using System.Linq;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    public class ClientService
    {
        private readonly IRepository _context;

        public ClientService(IRepository context)
        {
            _context = context;
        }

        public IEnumerable<Client> GetClientWithName(string name)
        {
            return _context.GetAll<Client>(null, c => c.Contactmen, c => c.Asignments)
                .Where(k => k.name == name);
        }
    }
}
