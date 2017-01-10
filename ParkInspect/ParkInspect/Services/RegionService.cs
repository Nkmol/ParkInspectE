using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
   public class RegionService : DataService
    {
        public RegionService(IRepository context) : base(context)
        {
            
        }
    }
}
