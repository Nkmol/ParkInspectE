using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    class AssignmentService : DataService
    {
        private readonly IRepository _context;

        public AssignmentService(IRepository context) : base(context)
        {
            _context = context;
        }
        /*
        public ObservableCollection<Tuple<IEnumerable<Parkinglot>, IEnumerable<Form>, IEnumerable<State>, IEnumerable<Inspection>, IEnumerable<Asignment>>> GetAllTuple()
        {
            var t = new Tuple<IEnumerable<Parkinglot>, IEnumerable<Form>, IEnumerable<State>, IEnumerable<Inspection>, IEnumerable<Asignment>>(GetAll<Parkinglot>(), GetAll<Form>(), GetAll<State>(), GetAll<Inspection>(), GetAll<Asignment>());
            return new ObservableCollection<Tuple<IEnumerable<Parkinglot>, IEnumerable<Form>, IEnumerable<State>, IEnumerable<Inspection>, IEnumerable<Asignment>>>(t);
        }
        */
        public IEnumerable<Asignment> GetAsignmentWithClient(string name)
        {
            return _context.GetAll<Asignment>(null, c => c.Inspections)
                .Where(k => k.Client.name == name);
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
