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
                Fields = new List<CachedFormField>(),
                TemplateId = fromTemplate.id
            };
            foreach(Field f in fromTemplate.Fields)
            {
                form.Fields.Add(new CachedFormField() { FieldTitle = f.title, Value = new CachedValue("[" + f.datatype + "]"), Datatype = f.datatype});
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
            form.template_id = cachedForm.TemplateId;
            foreach(string attachment in cachedForm.Attachments)
            {
                Image image = new Image()
                {
                    image1 = attachment
                };
                form.Image = image;
            }
            foreach(CachedFormField field in cachedForm.Fields)
            {
                Formfield formField = new Formfield()
                {
                    field_title = field.FieldTitle,
                    value = field.Value.ToString(),
                    field_template_id = cachedForm.TemplateId

                };
                Debug.WriteLine(field.FieldTitle + " : " + field.Value.ToString() + "(" + field.Value.Type + ")");
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
