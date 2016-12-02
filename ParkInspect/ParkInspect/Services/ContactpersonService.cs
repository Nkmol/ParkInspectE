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

        public IEnumerable<Client> GetAllClients()
        {
            return _context.GetAll<Client>();
        }

        public IEnumerable<Contactperson> GetContactperson(Contactperson c)
        {
            return _context.Get<Contactperson>().Where(cp => cp == c);
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

        public void DeleteContactpeson(Contactperson c)
        {
            _context.Delete(c);
            _context.Save();
        }
    }
}