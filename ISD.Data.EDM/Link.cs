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
    
    public partial class Link
    {
        public Link()
        {
            this.FooterMenu = new HashSet<FooterMenu>();
            this.Menu = new HashSet<Menu>();
        }
    
        public int ID { get; set; }
        public Nullable<int> LinkType { get; set; }
        public string LinkText { get; set; }
        public string LinkValue { get; set; }
    
        public virtual ICollection<FooterMenu> FooterMenu { get; set; }
        public virtual ICollection<Menu> Menu { get; set; }
    }
}
