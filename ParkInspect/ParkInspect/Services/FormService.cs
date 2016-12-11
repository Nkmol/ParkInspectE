using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;
using ParkInspect.ViewModel;
using System.Diagnostics;

namespace ParkInspect.Services
{
    class FormService
    {
        public EntityFrameworkRepository<ParkInspectEntities> central;
        private EntityFrameworkRepository<ParkInspectEntities> local;

        public FormService(EntityFrameworkRepository<ParkInspectEntities> central, EntityFrameworkRepository<ParkInspectEntities> local)
        {
            this.central = central;
            this.local = local;
        }

        public CachedForm createFormFromTemplate(Template fromTemplate)
        {
            CachedForm form = new CachedForm() {
                fields = new List<CachedFormField>(),
                template_id = fromTemplate.id
            };
            foreach(Field f in fromTemplate.Fields)
            {
                form.fields.Add(new CachedFormField() { field_title = f.title, value = new CachedValue("[" + f.datatype + "]"), datatype = f.datatype});
            }
            return form;
        }

        public void SaveForm(CachedForm cachedForm)
        {
            Form form = new Form();
            foreach(CachedFormField field in cachedForm.fields)
            {
                Formfield formField = new Formfield()
                {
                    field_title = field.field_title,
                    value = field.value.ToString(),
                    field_template_id = cachedForm.template_id

                };
                Debug.WriteLine(field.field_title + " : " + field.value.ToString() + "(" + field.value.type + ")");
                form.Formfields.Add(formField);
            }
            if (central.IsConnected())
            {
                //central.Create(form);
                //central.Save();
            }
            else
            {

                /*
                Form localForm = new ParkInspectEntities1.Form();
                localForm.field_title = form.field_title;
                localForm.value = form.value
                local.Create(localForm);
                */
            }
        }

    }
}
