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
    
    public partial class Reward
    {
        public Reward()
        {
            this.RewardImage1 = new HashSet<RewardImage>();
            this.RewardsDetails = new HashSet<RewardsDetails>();
            this.VoucherDetails = new HashSet<VoucherDetails>();
        }
    
        public int ID { get; set; }
        public System.Guid ProviderID { get; set; }
        public Nullable<int> RequiredRewardPoint { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public string RewardsName { get; set; }
        public int RewardType { get; set; }
        public Nullable<System.DateTime> RewardExpiryDate { get; set; }
        public Nullable<int> RequiredActivityEnroll { get; set; }
        public Nullable<int> NofTimeUsed { get; set; }
        public Nullable<int> Discount { get; set; }
        public Nullable<int> BonusPoint { get; set; }
        public Nullable<int> RewardSource { get; set; }
        public Nullable<int> SupportOFF { get; set; }
        public string FreeGift { get; set; }
        public Nullable<int> FreeActivityID { get; set; }
        public string RewardDescription { get; set; }
        public Nullable<int> UsageTimes { get; set; }
        public Nullable<bool> RewardImage { get; set; }
    
        public virtual Sponsor Sponsor { get; set; }
        public virtual RewardsType RewardsType { get; set; }
        public virtual ICollection<RewardImage> RewardImage1 { get; set; }
        public virtual ICollection<RewardsDetails> RewardsDetails { get; set; }
        public virtual ICollection<VoucherDetails> VoucherDetails { get; set; }
    }
}
