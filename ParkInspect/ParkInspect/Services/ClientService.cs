using System.Collections.Generic;
using System.Linq;
using ParkInspect.Repository;
using System.Collections.ObjectModel;

namespace ParkInspect.Services
{
    public class ClientService
    {
        private readonly IRepository _context;

        public ClientService(IRepository context)
        {
            _context = context;
        }
        public void AddClient(Client c)
        {
            _context.Create(c);
            _context.Save();
        }

        public IEnumerable<Client> GetAllClients()
        {
            return _context.GetAll<Client>(null, c => c.Contactman, c => c.Asignment);
        }       

        public IEnumerable<Client> GetClientWithName(string name)
        {
            return _context.GetAll<Client>(null, c => c.Contactman, c => c.Asignment)
                .Where(k => k.name == name);
        }
    }
}
