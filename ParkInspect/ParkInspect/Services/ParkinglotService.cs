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

        public IEnumerable<Region> GetAllRegions()
        {
            return _context.GetAll<Region>();
        }

        public IEnumerable<Inspection> GetAllInspections()
        {
            return _context.GetAll<Inspection>();
        }

        public IEnumerable<Parkinglot> GetAllParkinglotsWhere(Dictionary<string, string> filters)
        {

            var query = _context.GetAll<Parkinglot>();

            foreach (var property in filters.Keys)
            {
                var filter = filters[property];
                filter = filter?.ToLower() ?? "";

                query = query.Where(x => 
                    (x.GetType().GetProperty(property).GetValue(x) == typeof(int) 
                    ? Convert.ToInt32(x.GetType().GetProperty(property).GetValue(x)) == Convert.ToInt32(filter) 
                    : Convert.ToString(x.GetType().GetProperty(property).GetValue(x)).ToLower().Contains(filter)));

            }

            return query;

        }
    }
}
