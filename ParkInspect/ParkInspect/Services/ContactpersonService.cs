using System.Collections.Generic;
using System.Linq;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    public class ContactpersonService
    {
        private readonly IRepository _context;

        public ContactpersonService(IRepository context)
        {
            _context = context;
        }

        public IEnumerable<Contactperson> GetAllContactpersons()
        {
            return _context.GetAll<Contactperson>();
        }

        public void AddContactperson(Contactperson c)
        {
            _context.Create(c);
            _context.Save();
        }

        public void UpdateContactperson(Contactperson c)
        {
            _context.Update(c);
            _context.Save();
        }

        public IEnumerable<string> GetAllClients()
        {
            return _context.GetAll<Client>().Select(c => c.name);
        }

        public int GetClientIdFromName(string name)
        {
            return _context.Get<Client>().Single(c => c.name == name).id;
        }
    }
}