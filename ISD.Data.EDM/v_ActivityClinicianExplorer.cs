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
    
    public partial class v_ActivityClinicianExplorer
    {
        public Nullable<long> ID { get; set; }
        public int ACID { get; set; }
        public Nullable<int> ActivityID { get; set; }
        public string ProviderID { get; set; }
        public string Name { get; set; }
        public Nullable<int> ClinicianID { get; set; }
        public string ClinicianName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<int> TimetableType { get; set; }
        public string Location { get; set; }
        public string Specialties { get; set; }
        public Nullable<bool> OnMonday { get; set; }
        public Nullable<bool> OnTuesday { get; set; }
        public Nullable<bool> OnWednesday { get; set; }
        public Nullable<bool> OnThursday { get; set; }
        public Nullable<bool> OnFriday { get; set; }
        public Nullable<bool> OnSaturday { get; set; }
        public Nullable<bool> OnSunday { get; set; }
        public Nullable<int> RecurEvery { get; set; }
        public string SavedName { get; set; }
        public Nullable<int> LocationID { get; set; }
    }
}
