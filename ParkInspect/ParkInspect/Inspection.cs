//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ParkInspect
{
    using System;
    using System.Collections.Generic;
    
    public partial class Inspection
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Inspection()
        {
            this.Inspection1 = new HashSet<Inspection>();
            this.Employees = new HashSet<Employee>();
        }
    
        public int id { get; set; }
        public int parking_id { get; set; }
        public Nullable<int> form_id { get; set; }
        public Nullable<int> follow_up_id { get; set; }
        public string state { get; set; }
        public Nullable<System.DateTime> deadline { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public string clarification { get; set; }
        public int assigment_client_id { get; set; }
        public int assignment_id { get; set; }
    
        public virtual Asignment Asignment { get; set; }
        public virtual Form Form { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inspection> Inspection1 { get; set; }
        public virtual Inspection Inspection2 { get; set; }
        public virtual Parkinglot Parkinglot { get; set; }
        public virtual State State1 { get; set; }
        public virtual Report Report { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employee> Employees { get; set; }
    }
}
