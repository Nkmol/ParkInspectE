using System;
using System.Collections.Generic;
using System.Linq;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    public class ClientService : DataService
    {
        public ClientService(IRepository context) : base(context)
        {
        }

        public IEnumerable<Client> GetClientsWithName(string name)
        {
            return Context.GetAll<Client>(null, c => c.Contactpersons, c => c.Asignments)
                .Where(k => k.name == name);
        }

        public bool Login(string email, string password)
        {
            return Context.Get<Client>(c => c.email == email && c.password == password).Count() != 0;
        }
    }
}
