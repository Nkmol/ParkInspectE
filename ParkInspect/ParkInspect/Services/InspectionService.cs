using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    public class InspectionService
    {
        private readonly IRepository _context;

        public InspectionService(IRepository context)
        {
            _context = context;
        }

        public IEnumerable<Inspection> GetInspectionWithId(int id)
        {
            return _context.GetAll<Inspection>(null, c => c.Employees, k => k.Inspection1)
                .Where(k => k.id == id);
        }

        public IEnumerable<Inspection> GetAllInspections()
        {
            return _context.GetAll<Inspection>();
        }
        public IEnumerable<Employee> GetAllInspecteurs()
        {
            return _context.GetAll<Employee>().Where(e => e.role == "Inspector");
        }


        public IEnumerable<Form> GetAllForms()
        {
            return _context.GetAll<Form>();
        }

        public IEnumerable<State> GetAllStates()
        {
            return _context.GetAll<State>();

        }

        public IEnumerable<Parkinglot> GetAllParkinglots()
        {
            return _context.GetAll<Parkinglot>();

        }

        public IEnumerable<Asignment> GetallAsignments()
        {
            return _context.GetAll<Asignment>();
        }
        public void UpdateInspection(Inspection inspection)
        {

            _context.Update(inspection);
            _context.Save();

        }
        public void CreateNewAssignemnt(Inspection inspection)
        {
            _context.Create(inspection);
            _context.Save();
        }
    }
}
