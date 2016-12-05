using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    public abstract class DataService
    {

        protected IRepository Context;

        protected DataService(IRepository context)
        {
            Context = context;
        }

        public abstract IEnumerable GetData(List<string> columns, List<string> alias);

    }
}
