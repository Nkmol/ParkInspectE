using System.Collections.Generic;
using System.Linq;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    public class ContactpersonService : DataService
    {
        public ContactpersonService(IRepository context) : base(context)
        {
        }

        public IEnumerable<Contactperson> GetContactperson(Contactperson c)
        {
            return Context.Get<Contactperson>().Where(cp => cp == c);
        }
    }
}