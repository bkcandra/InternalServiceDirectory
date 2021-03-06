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
    
    public partial class v_ActivityExplorer
    {
        public int ID { get; set; }
        public virtual string Name { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual Nullable<int> SuburbID { get; set; }
        public virtual Nullable<int> StateID { get; set; }
        public virtual string Suburb { get; set; }
        public virtual string ShortDescription { get; set; }
        public virtual string StateName { get; set; }
        public virtual Nullable<int> CategoryID { get; set; }
        public virtual string CategoryName { get; set; }
        public virtual Nullable<int> CategoryLevel1ParentID { get; set; }
        public virtual string CategoryLevel1ParentName { get; set; }
        public virtual Nullable<int> CategoryLevel2ParentID { get; set; }
        public virtual string CategoryLevel2ParentName { get; set; }
        public virtual string Address { get; set; }
        public virtual Nullable<int> PostCode { get; set; }
        public int Status { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public virtual Nullable<int> ActivityType { get; set; }
        public virtual string Keywords { get; set; }
        public virtual string Website { get; set; }
        public virtual string Price { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string MiddleName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Email { get; set; }
        public virtual string MobileNumber { get; set; }
        public virtual Nullable<int> Title { get; set; }
        public virtual Nullable<int> AltTitle { get; set; }
        public virtual string AltFirstName { get; set; }
        public virtual string AltMiddleName { get; set; }
        public virtual string AltLastName { get; set; }
        public virtual string AltEmail { get; set; }
        public virtual string AltAddress { get; set; }
        public virtual Nullable<int> AltSuburbID { get; set; }
        public virtual Nullable<int> AltStateID { get; set; }
        public virtual Nullable<int> AltPostCode { get; set; }
        public virtual string AltPhoneNumber { get; set; }
        public virtual string AltMobileNumber { get; set; }
        public virtual string FullDescription { get; set; }
        public Nullable<System.DateTime> ModifiedDateTime { get; set; }
        public virtual Nullable<int> TimetableSize { get; set; }
        public virtual Nullable<int> TimetableType { get; set; }
        public Nullable<bool> Personalised { get; set; }
        public virtual string ProviderName { get; set; }
        public virtual string ProviderBranch { get; set; }
        public virtual Nullable<int> AgeTo { get; set; }
        public virtual Nullable<int> AgeFrom { get; set; }
        public Nullable<bool> forMale { get; set; }
        public Nullable<bool> forFemale { get; set; }
        public Nullable<bool> forChildren { get; set; }
        public virtual Nullable<int> IsPaid { get; set; }
        public Nullable<bool> isApproved { get; set; }
        public virtual Nullable<int> SecondaryCategoryID1 { get; set; }
        public virtual Nullable<int> SecondaryCategoryID2 { get; set; }
        public virtual Nullable<int> SecondaryCategoryID3 { get; set; }
        public virtual Nullable<int> SecondaryCategoryID4 { get; set; }
        public virtual string Council { get; set; }
        public virtual Nullable<int> CouncilID { get; set; }
        public virtual string ReferenceID { get; set; }
        public virtual Nullable<int> Visitor { get; set; }
        public virtual string ProviderID { get; set; }
        public Nullable<bool> isPrimary { get; set; }
        public virtual Nullable<int> PrimaryServiceID { get; set; }
        public virtual string Note { get; set; }
        public virtual string Assessment { get; set; }
        public Nullable<bool> Eligibility { get; set; }
        public virtual string Requirements { get; set; }
    }
}
