using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    public class TemplateService
    {
        public EntityFrameworkRepository<ParkInspectEntities> central;
        private EntityFrameworkRepository<ParkInspectEntities> local;

        public TemplateService(EntityFrameworkRepository<ParkInspectEntities> central, EntityFrameworkRepository<ParkInspectEntities> local)
        {
            this.central = central; 
            this.local = local;
        }

        public Template createTemplate()
        {
            Template template = new Template() { version_number = "1.0" };
            return template;
        }

        public Template editTemplate(Template source)
        {
            Template template = new Template();
            template.name = source.name;
            template.version_number = getNextVersion(source);
            foreach (Field f in source.Fields)
            {
                template.Fields.Add(new Field() { title = f.title, datatype = f.datatype });
            }
            return template;
        }

        public string getNextVersion(Template source)
        {
            string nextVersion = (int.Parse(source.version_number.Replace(".0", "")) + 1).ToString() + ".0";
            if (templateExists(source.name, nextVersion)) // if next version exists start sub numbering
            {
                int subVersion = 1;
                while (templateExists(source.name,int.Parse(source.version_number.Replace(".0", "")).ToString() + "." +  subVersion.ToString())){
                    subVersion++;
                }
                nextVersion = int.Parse(source.version_number.Replace(".0", "")).ToString() + "." + subVersion.ToString();
            }
            return nextVersion;
        }

        public bool templateExists(string name, string version)
        {
            foreach (Template t in central.GetAll<Template>())
            {
                if (t.name == name && t.version_number == version)
                {
                    return true; 
                }
            }
            return false;
        }

        public void SaveTemplate(Template template)
        {
            if (central.IsConnected())
            {
                central.Create(template);
                central.Save();
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
