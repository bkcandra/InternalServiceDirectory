//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ISD.Data.EDM
{
    using System;
    using System.Collections.Generic;
    
    public partial class Council
    {
        public Council()
        {
            this.Suburb = new HashSet<Suburb>();
        }
    
        public int ID { get; set; }
        public Nullable<int> StateID { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> CreatedDatetime { get; set; }
        public Nullable<System.DateTime> ModifiedDatetime { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
    
        public virtual State State { get; set; }
        public virtual ICollection<Suburb> Suburb { get; set; }
    }
}
