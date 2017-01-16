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
    
    public partial class LocalEmployee
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LocalEmployee()
        {
            this.Absence = new HashSet<LocalAbsence>();
            this.Inspection = new HashSet<LocalInspection>();
        }
    
        public int id { get; set; }
        public string employee_status { get; set; }
        public string role { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public bool active { get; set; }
        public string phonenumber { get; set; }
        public System.DateTime in_service_date { get; set; }
        public Nullable<System.DateTime> out_service_date { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocalAbsence> Absence { get; set; }
        public virtual LocalEmployee_Status Employee_Status1 { get; set; }
        public virtual LocalRole Role1 { get; set; }
        public virtual LocalRegion Region { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocalInspection> Inspection { get; set; }
    }
}
