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

        public bool AddParkinglot(Parkinglot p)
        {

            try
            {
                Context.Create(p);
                Context.Save();
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
                Context.Update(p);
                Context.Save();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public IEnumerable<Parkinglot> GetAllParkinglots()
        {
            return Context.GetAll<Parkinglot>();
        }

        public IEnumerable<Region> GetAllRegions()
        {
            return Context.GetAll<Region>();
        }

        public IEnumerable<Inspection> GetAllInspections()
        {
            return Context.GetAll<Inspection>();
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

        public IEnumerable GetData(List<string> columns, List<string> alias)
        {

            var query = Context.GetAll<Parkinglot>().AsEnumerable().Select(x =>
            {
                var data = new ExpandoObject();
                for (int i = 0; i < columns.Count; i++)
                {
                    ((IDictionary<String, Object>)data)
                     .Add((alias != null ? alias[i] : columns[i]), x.GetType().GetProperty(columns[i]).GetValue(x));
                }
                return data;
            });
            return query;

        }

    }
}
