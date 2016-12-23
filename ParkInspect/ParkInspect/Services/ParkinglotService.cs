using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    public class ParkinglotService : DataService
    {
        private readonly IRepository _context;

        public ParkinglotService(IRepository context) : base(context)
        {
            _context = context;
        }

        public Parkinglot Get(Parkinglot pl)
        {
            return _context.Get<Parkinglot>().FirstOrDefault(x => x == pl);
        }

        public Parkinglot GetParkinglot(string name)
        {
            return _context.Get<Parkinglot>().FirstOrDefault(x => x.name == name);
        }
    }
}
