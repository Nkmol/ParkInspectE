using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    class TemplateService
    {
        EntityFrameworkRepository<ParkInspectEntities> central;
        EntityFrameworkRepository<ParkInspectEntities> local;

        public TemplateService(EntityFrameworkRepository<ParkInspectEntities> central, EntityFrameworkRepository<ParkInspectEntities> local)
        {
            this.central = central;
            this.local = local;
        }

        public Template createTemplate()
        {
            Template template = new Template();
            return template;
        }

        public Template editTemplate(Template source)
        {
            Template template = new Template();
            template.name = source.name;
            template.version_number = source.version_number + 1;
            foreach(Field f in source.Fields)
            {
                template.Fields.Add(new Field() { title = f.title, datatype = f.datatype });
            }
            return template;
        }

        public void SaveTemplate(Template template)
        {
            if (central.IsConnected())
            {
                central.Create(template);
                central.Save();
                throw new Exception();
            } else
            {

                /*
                Template localTemplate = new ParkInspectEntities1.Template();
                localTemplate.name = template.name;
                localTemplate.version_number = template.version_number
                local.Create<Template>(localTemplate);
                */
            }
        }
    }
}
