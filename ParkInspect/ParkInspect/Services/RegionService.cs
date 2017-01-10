using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
   public class RegionService
    {
        private readonly IRepository _context;

        public RegionService(IRepository context)
        {
            _context = context;
        }

        public IEnumerable<Region> GetAllRegions()
        {
            return _context.GetAll<Region>();
        }

        public void InsertRegion(Region r)
        {
            _context.Create(r);
            _context.Save();
        }
    }
}
