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

        public IEnumerable<T> GetAll<T>() where T : class
        {
            return Context.GetAll<T>();
        }

        public bool Add<T>(T p) where T : class
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

        public bool Update<T>(T p) where T : class
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

    }
}
