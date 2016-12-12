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

        public ParkinglotService(IRepository context) : base(context)
        {
        }

    }
}
