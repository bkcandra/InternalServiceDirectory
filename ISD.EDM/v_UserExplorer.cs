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
    
    public partial class v_UserExplorer
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Email { get; set; }
        public string ReferenceID { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string MobileNumber { get; set; }
        public Nullable<System.DateTime> CreatedDatetime { get; set; }
        public Nullable<System.DateTime> ModifiedDatetime { get; set; }
        public Nullable<int> PreferredContact { get; set; }
    }
}
