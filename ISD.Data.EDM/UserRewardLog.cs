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
    
    public partial class UserRewardLog
    {
        public int ID { get; set; }
        public Nullable<int> UserRewardID { get; set; }
        public int UserRewardLogType { get; set; }
        public Nullable<int> LogActionID { get; set; }
        public Nullable<int> PointValue { get; set; }
        public Nullable<int> BonusPoint { get; set; }
        public string Message { get; set; }
    
        public virtual UserReward UserReward { get; set; }
    }
}