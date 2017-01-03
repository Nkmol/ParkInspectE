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

        public IEnumerable<Asignment> GetAsignmentWithClient(string name)
        {
            return _context.GetAll<Asignment>(null, c => c.Inspections)
                .Where(k => k.Client.name == name);
        }

        public bool InsertOrUpdate(AssignmentViewModel viewModel)
        {
            // Combine ModelViews with POCO objects
            foreach (var inspection in viewModel.Inspections)
                viewModel.Data.Inspections.Add(inspection.Data);

            return InsertOrUpdate(viewModel.Data);
        }
    }
}
