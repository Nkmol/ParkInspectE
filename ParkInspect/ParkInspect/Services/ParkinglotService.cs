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

    }
}
