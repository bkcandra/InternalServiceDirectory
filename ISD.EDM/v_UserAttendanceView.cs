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
    
    public partial class v_UserAttendanceView
    {
        public int ID { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public int ActivityID { get; set; }
        public string Name { get; set; }
        public string FullDescription { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<System.Guid> ProviderId { get; set; }
        public Nullable<int> TotalHours { get; set; }
        public Nullable<int> CaloriesPerHour { get; set; }
        public Nullable<int> Earnrewards { get; set; }
        public Nullable<int> Tapreq { get; set; }
        public Nullable<int> QRCodeID { get; set; }
        public Nullable<bool> BpFlag { get; set; }
        public Nullable<int> BonusPoints { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public string CreatedBy { get; set; }
    }
}
