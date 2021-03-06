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
    
    public partial class Activity
    {
        public Activity()
        {
            this.ActivitiesLogGroup = new HashSet<ActivitiesLogGroup>();
            this.ActivityUserAttendance = new HashSet<ActivityUserAttendance>();
            this.ActivityClinician = new HashSet<ActivityClinician>();
            this.ActivityContactDetail = new HashSet<ActivityContactDetail>();
            this.ActivityGrouping = new HashSet<ActivityGrouping>();
            this.ActivityImage = new HashSet<ActivityImage>();
            this.ActivityReferenceCode = new HashSet<ActivityReferenceCode>();
            this.ActivityRewards = new HashSet<ActivityRewards>();
            this.ActivitySchedule = new HashSet<ActivitySchedule>();
            this.ActivityEligibility = new HashSet<ActivityEligibility>();
            this.ActivityVisitor = new HashSet<ActivityVisitor>();
        }
    
        public int ID { get; set; }
        public string ProviderID { get; set; }
        public string ActivityCode { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public Nullable<int> SecondaryCategoryID1 { get; set; }
        public Nullable<int> SecondaryCategoryID2 { get; set; }
        public Nullable<int> SecondaryCategoryID3 { get; set; }
        public Nullable<int> SecondaryCategoryID4 { get; set; }
        public string Price { get; set; }
        public Nullable<System.DateTime> CreatedDateTime { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public Nullable<int> ActivityType { get; set; }
        public int Status { get; set; }
        public string Website { get; set; }
        public string Keywords { get; set; }
        public Nullable<int> TimetableType { get; set; }
        public Nullable<bool> isApproved { get; set; }
        public Nullable<bool> isPrimary { get; set; }
        public Nullable<int> PrimaryServiceID { get; set; }
        public Nullable<bool> Eligibility { get; set; }
    
        public virtual ICollection<ActivitiesLogGroup> ActivitiesLogGroup { get; set; }
        public virtual ICollection<ActivityUserAttendance> ActivityUserAttendance { get; set; }
        public virtual ICollection<ActivityClinician> ActivityClinician { get; set; }
        public virtual ICollection<ActivityContactDetail> ActivityContactDetail { get; set; }
        public virtual ICollection<ActivityGrouping> ActivityGrouping { get; set; }
        public virtual ICollection<ActivityImage> ActivityImage { get; set; }
        public virtual ICollection<ActivityReferenceCode> ActivityReferenceCode { get; set; }
        public virtual ICollection<ActivityRewards> ActivityRewards { get; set; }
        public virtual ICollection<ActivitySchedule> ActivitySchedule { get; set; }
        public virtual ICollection<ActivityEligibility> ActivityEligibility { get; set; }
        public virtual ICollection<ActivityVisitor> ActivityVisitor { get; set; }
    }
}
