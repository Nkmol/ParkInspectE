//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ParkInspect.Model.LocalModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class LocalContactperson
    {
        public int id { get; set; }
        public int client_id { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
    
        public virtual LocalClient Client { get; set; }
    }
}
