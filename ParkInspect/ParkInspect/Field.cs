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
    
    public partial class Field
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Field()
        {
            this.Formfields = new HashSet<Formfield>();
        }
    
        public string title { get; set; }
        public int template_id { get; set; }
        public string datatype { get; set; }
        public string reportFieldType_title { get; set; }
    
        public virtual Datatype Datatype1 { get; set; }
        public virtual ReportFieldType ReportFieldType { get; set; }
        public virtual Template Template { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Formfield> Formfields { get; set; }
    }
}
