using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkInspect.Model.Factory.Builder
{
    public class FilterBuilder
    {

        private Dictionary<string, object> filters { get; set; }

        public FilterBuilder()
        {
            filters = new Dictionary<string, object>();
        }

        public void Add(string key, object obj)
        {
            filters.Add(key, obj);
        }

        public Dictionary<string, object> Get()
        {
            return filters;
        }

    }
}
