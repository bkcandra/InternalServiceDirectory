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
    
    public partial class ProviderSettings
    {
        public string ProviderID { get; set; }
        public Nullable<bool> Personalised { get; set; }
        public Nullable<int> TimetableType { get; set; }
        public Nullable<int> TimetableSize { get; set; }
    
        public virtual ProviderProfiles ProviderProfiles { get; set; }
    }
}
