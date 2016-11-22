using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    public class ParkinglotService
    {

        private readonly IRepository _context;

        public ParkinglotService(IRepository context)
        {
            _context = context;
        }

        public bool AddParkinglot(Parkinglot p)
        {

            try
            {
                _context.Create(p);
                _context.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }

        public bool UpdateParkinglot(Parkinglot p)
        {
            try
            {
                _context.Update(p);
                _context.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<Parkinglot> GetAllParkinglots()
        {
            return _context.GetAll<Parkinglot>();
        }

        public IEnumerable<Parkinglot> GetParkinglotByName(string name)
        {
            return _context.GetAll<Parkinglot>().Where(p => p.name == name);
        }

        public IEnumerable<Parkinglot> GetParkinglotByZip(string zip)
        {
            return _context.GetAll<Parkinglot>().Where(p => p.zipcode == zip);
        }

    }
}
