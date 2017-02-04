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
        public IRepository repo;

        public FormService(IRepository repo)
        {
            this.repo = repo;
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

        public void SaveForm(Inspection inspection,CachedForm cachedForm,bool isNew)
        {
            Form form = new Form();
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
            if (isNew)
            {
                inspection.Form = form;
                repo.Create(form);
                repo.Save();
            } else
            {
                Form oldForm = repo.Get<Inspection>(x => x.id == inspection.id).First().Form;
                foreach (Formfield oldFormField in oldForm.Formfields)
                {
                    oldFormField.value = (form.Formfields.First(x => x.field_title == oldFormField.field_title).value);
                    Debug.WriteLine(oldFormField.value);
                }
                Debug.WriteLine("IS NOT NEW");
                repo.Save();
            }
        }

    }
}
