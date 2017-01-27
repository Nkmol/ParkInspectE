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
        public IRepository central;
        private EntityFrameworkRepository<ParkInspectEntities> local;

        public FormService(IRepository central)
        {
            this.central = central;
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

        public CachedForm createCachedFormFromForm(Form form)
        {
            CachedForm cachedForm = new CachedForm();

            return cachedForm;
        }

        public void SaveForm(Inspection inspection,CachedForm cachedForm)
        {
            Form form = new Form();
            inspection.Form = form;
            form.template_id = cachedForm.template_id;
            foreach(string attachment in cachedForm.attachments)
            {
                Image image = new Image()
                {
                    image1 = attachment
                };
                form.Image = image;
            }
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
            //if (central.IsConnected())
            //{
                central.Create(form);
                central.Save();
            /*}
            else
            {
                using (var context = new ParkInspectLocalEntities())
                {
                    context.Forms.Add(form);
                    context.SaveChanges();
                }
            }
            */
        }

    }
}
