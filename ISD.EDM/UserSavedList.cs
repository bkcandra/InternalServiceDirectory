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
    
    public partial class UserSavedList
    {
        public int ID { get; set; }
        public string OwnerGuid { get; set; }
        public int ListType { get; set; }
        public int ListValue { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDatetime { get; set; }
    
        public virtual UserProfiles UserProfiles { get; set; }
    }
}