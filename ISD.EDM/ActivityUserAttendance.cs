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
    
    public partial class ActivityUserAttendance
    {
        public int ID { get; set; }
        public System.Guid UserID { get; set; }
        public int ActivityID { get; set; }
        public Nullable<System.DateTime> AttendanceDatetime { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<bool> Processed { get; set; }
        public Nullable<System.DateTime> ProcesssedDatetime { get; set; }
    
        public virtual Activity Activity { get; set; }
    }
}