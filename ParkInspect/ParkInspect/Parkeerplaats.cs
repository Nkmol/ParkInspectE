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
    
    public partial class Parkeerplaats
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Parkeerplaats()
        {
            this.Inspectie = new HashSet<Inspectie>();
        }
    
        public int id { get; set; }
        public string regio_naam { get; set; }
        public string postcode { get; set; }
        public Nullable<int> nummer { get; set; }
        public string naam { get; set; }
        public string toelichting { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Inspectie> Inspectie { get; set; }
        public virtual Regio Regio { get; set; }
    }
}
