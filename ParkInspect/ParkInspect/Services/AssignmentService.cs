using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;
using ParkInspect.ViewModel.AssignmentVM;

namespace ParkInspect.Services
{
    class AssignmentService : DataService
    {
        private readonly IRepository _context;

        public AssignmentService(IRepository context) : base(context)
        {
            _context = context;
        }

        private AssignmentViewModel Combine(AssignmentViewModel viewModel)
        {
            viewModel.Data.Inspections.Clear();
            foreach (var inspection in viewModel.Inspections)
            {
                inspection.Data.Employees.Clear();
                // Add inspectors to POCO inspection
                foreach (var inspector in inspection.AssignedInspectors)
                {
                    inspection.Data.Employees.Add(inspector);
                }
                // Add Inspections to POCO assignment
                viewModel.Data.Inspections.Add(inspection.Data);
            }

            return viewModel;
        }

        public IEnumerable<Asignment> GetAsignmentWithClient(string name)
        {
            return _context.GetAll<Asignment>(null, c => c.Inspections)
                .Where(k => k.Client.name == name);
        }

        public bool Add(AssignmentViewModel viewModel)
        {
            viewModel = Combine(viewModel);
            return Add(viewModel.Data);
        }

        public bool Update(AssignmentViewModel viewModel)
        {
            viewModel = Combine(viewModel);

            // Remove unassigned/removed inspections
            foreach (var inspection in viewModel.UnassignedInspections)
            {
                Delete(inspection.Data);
            }

            return Update(viewModel.Data);
        }
    }
}
