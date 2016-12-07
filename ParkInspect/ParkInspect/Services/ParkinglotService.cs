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

        public IEnumerable<Parkinglot> GetAllParkinglotsWhere(Dictionary<string, string> filters)
        {

            var query = Context.GetAll<Parkinglot>();

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
