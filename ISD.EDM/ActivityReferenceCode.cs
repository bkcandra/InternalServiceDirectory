//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ISD.EDM
{
    using System;
    using System.Collections.Generic;
    
    public partial class ActivityReferenceCode
    {
        public int ID { get; set; }
        public int ActivityID { get; set; }
        public Nullable<System.Guid> ActivityGUID { get; set; }
        public string ReferenceID { get; set; }
    
        public virtual Activity Activity { get; set; }
    }
}
