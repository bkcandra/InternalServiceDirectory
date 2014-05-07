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
    
    public partial class v_ActivityView
    {
        public int ID { get; set; }
        public string ActivityCode { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string Price { get; set; }
        public Nullable<System.DateTime> ActivityCreatedDateTime { get; set; }
        public Nullable<System.DateTime> ActivityModifiedDateTime { get; set; }
        public string ActivityModifiedBy { get; set; }
        public string ActivityCreatedBy { get; set; }
        public Nullable<int> Title { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public Nullable<int> SuburbID { get; set; }
        public Nullable<int> StateID { get; set; }
        public Nullable<int> PostCode { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<int> AltPostCode { get; set; }
        public string AltPhoneNumber { get; set; }
        public string AltMobileNumber { get; set; }
        public Nullable<bool> forMale { get; set; }
        public Nullable<bool> forFemale { get; set; }
        public Nullable<bool> forChildren { get; set; }
        public Nullable<int> AgeFrom { get; set; }
        public Nullable<int> AgeTo { get; set; }
        public string Expr1 { get; set; }
        public string CategoryName { get; set; }
        public Nullable<int> CategoryLevel1ParentID { get; set; }
        public string CategoryLevel1ParentName { get; set; }
        public Nullable<int> CategoryLevel2ParentID { get; set; }
        public string CategoryLevel2ParentName { get; set; }
        public Nullable<System.Guid> UserID { get; set; }
        public string Suburb { get; set; }
        public string StateName { get; set; }
        public Nullable<System.Guid> ProviderID { get; set; }
        public string Website { get; set; }
        public string Keywords { get; set; }
        public Nullable<int> AltStateID { get; set; }
        public Nullable<int> AltSuburbID { get; set; }
        public string AltAddress { get; set; }
        public string AltEmail { get; set; }
        public string AltLastName { get; set; }
        public string AltMiddleName { get; set; }
        public string AltFirstName { get; set; }
        public Nullable<int> AltTitle { get; set; }
        public string eligibilityDescription { get; set; }
        public Nullable<int> ActivityID { get; set; }
        public int Status { get; set; }
    }
}
