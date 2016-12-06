using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;
using System.Dynamic;
using System.Runtime.InteropServices;

namespace ParkInspect.Services
{
    public abstract class DataService
    {

        protected IRepository Context;

        protected DataService(IRepository context)
        {
            Context = context;
        }

        /*
         * Get all data, corresponding to the columns and aliases, using ExpandoObjects.
         */
        public IEnumerable GetData<T>(List<string> columns, List<string> alias) where T : class
        {

            var query = Context.GetAll<T>().AsEnumerable().Select(x =>
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
