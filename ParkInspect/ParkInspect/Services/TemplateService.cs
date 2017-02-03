﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkInspect.Repository;

namespace ParkInspect.Services
{
    public class TemplateService
    {
        public IRepository central;
        private EntityFrameworkRepository<ParkInspectLocalEntities> local;

        public TemplateService(IRepository central)
        {
            this.central = central; 
            this.local = ViewModel.ViewModelLocator.localRepo;
        }

        public Template createTemplate()
        {
            Template template = new Template() { version_number = "1" };
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
            string ogVersion = source.version_number;
            string[] digits = ogVersion.Split('.');
            int smallest = int.Parse(digits[digits.Length - 1]);
            digits[digits.Length - 1] = null;
            string nextVersion = "";
            foreach(string digit in digits)
            {
                if (digit == null)
                {
                    break;
                }
                nextVersion += digit + ".";
            }
            if (templateExists(source.name,nextVersion + (smallest + 1)))
            {
                string nextBase = nextVersion + (smallest);
                int counter = 1;
                string version = nextBase + "." + counter;
                while (templateExists(source.name, version))
                {
                    counter++;
                    version = nextBase + "." + counter;
                }
                return version;
            }
            return nextVersion + (smallest + 1);
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
            if (central is EntityFrameworkReadOnlyRepository<ParkInspectEntities> && ((EntityFrameworkReadOnlyRepository < ParkInspectEntities > )central).IsConnected())
            {
                central.Create(template);
                central.Save();
            } else
            {
                using (var context = new ParkInspectLocalEntities())
                {
                    context.Templates.Add(template);
                    context.SaveChanges();
                }
            }
        }
    }
}
