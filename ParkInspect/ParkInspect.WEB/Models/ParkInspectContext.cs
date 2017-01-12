using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ParkInspect.WEB.Models
{
    public class ParkInspectContext : DbContext
    {
        public ParkInspectContext(string connectionstring) : base (connectionstring) { }
    }
}