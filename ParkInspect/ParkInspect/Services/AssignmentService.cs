using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    class AssignmentService
    {
        private readonly IRepository _context;

        public AssignmentService(IRepository context)
        {
            _context = context;
        }

        public IEnumerable<Asignment> GetAsignmentWithClient(string name)
        {
            return _context.GetAll<Asignment>(null, c => c.Inspections)
                .Where(k => k.Client.name == name);
        }

        public IEnumerable<Asignment> GetAllAsignments()
        {
            return _context.GetAll<Asignment>();
        }

        public IEnumerable<State> GetAllStates()
        {
            return _context.GetAll<State>();
        }

        public IEnumerable<Client> GetAllClients()
        {
            return _context.GetAll<Client>();
        }

        public IEnumerable<Inspection> GetAllInspections()
        {
            return _context.GetAll<Inspection>();
        }


        public void UpdateAssignment(Asignment assignment)
        {

            _context.Update(assignment);
            _context.Save();

        }

        public void DeleteAssignment(Asignment assignment)
        {
            _context.Delete(assignment);
            _context.Save();

        }

        public void CreateNewAssignemnt(Asignment assignment)
        {
            _context.Create(assignment);
            _context.Save();
        }
    }
}
