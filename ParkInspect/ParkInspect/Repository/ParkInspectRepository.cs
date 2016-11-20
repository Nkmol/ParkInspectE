using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkInspect.Repository
{
    class ParkInspectRepository : EntityFrameworkRepository<ParkInspectModels>
    {
        public ParkInspectRepository(ParkInspectModels context) : base(context)
        {
        }
    }
}
