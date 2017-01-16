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
    
    public partial class LocalInspection
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LocalInspection()
        {
            this.Inspection1 = new HashSet<LocalInspection>();
            this.Employee = new HashSet<LocalEmployee>();
        }
    
        public int id { get; set; }
        public int parking_id { get; set; }
        public Nullable<int> form_id { get; set; }
        public Nullable<int> follow_up_id { get; set; }
        public string state { get; set; }
        public Nullable<System.DateTime> deadline { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string clarification { get; set; }
        public int assignment_id { get; set; }
    
        public virtual LocalAsignment Asignment { get; set; }
        public virtual LocalForm Form { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocalInspection> Inspection1 { get; set; }
        public virtual LocalInspection Inspection2 { get; set; }
        public virtual LocalParkinglot Parkinglot { get; set; }
        public virtual LocalState State1 { get; set; }
        public virtual LocalReport Report { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LocalEmployee> Employee { get; set; }
    }
}
