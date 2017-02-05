using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;
using ParkInspect.ViewModel;
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

                // tranform Template to Form, once user confirms choice
                if (inspection.SelectedTemplate != null)
                {
                    // If a Form was already saved, remove old one first
                    if (inspection.Form != null)
                    {
                        var fields = inspection.Form.Formfields.ToList();
                        foreach (var formFields in fields)
                        {
                            Delete(formFields);
                        }
                        Delete(inspection.Form);

                        //Delete old Attachements of Form
                        var path = $@"{FormViewModel.FileImagesPath}\{inspection.Form.id}";
                        if (Directory.Exists(path))
                        {
                            Directory.Delete(path, true);
                        }

                        inspection.Form = null;
                    }

                    inspection.Form = new Form() {template_id = inspection.SelectedTemplate.id};

                    // Assign assiociated fields
                    foreach (var templateField in inspection.SelectedTemplate.Fields)
                    {
                        inspection.Form.Formfields.Add(item: new Formfield()
                        {
                            field_template_id = inspection.SelectedTemplate.id,
                            Form = inspection.Form,
                            Field = templateField,
                            field_title = templateField.title,
                            value = "[" + templateField.datatype + "]",
                        });
                    }
                }
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
