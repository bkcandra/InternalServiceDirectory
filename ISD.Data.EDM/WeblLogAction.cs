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
    
    public partial class WeblLogAction
    {
        public int ID { get; set; }
        public Nullable<int> ActionType { get; set; }
        public Nullable<int> WebLogID { get; set; }
        public string Message { get; set; }
        public Nullable<int> LogCategory { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public string Value { get; set; }
    
        public virtual WebLog WebLog { get; set; }
    }
}
