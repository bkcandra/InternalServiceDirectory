using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ISD.Data.EDM;
using ISD.Util;
using Microsoft.Ajax.Utilities;

namespace ISD.Application.provider.MVC.Models
{
    public class ServiceDetailModel : ISD.Data.EDM.v_ActivityExplorer
    {
        public SystemConstants.FormMode Mode { get; set; }
        public List<ActivityImageDetail> Images { get; set; }
        public List<v_ActivityExplorer> Services { get; set; }
        public List<v_ActivityClinicianExplorer> Clinicians { get; set; }
        public List<v_ProviderClinicians> CliniciansList { get; set; }
        public List<State> States { get; set; }
        public List<Suburb> Suburbs { get; set; }
        public List<v_CategoryExplorer> Categories { get; set; }
        public ICollection<int> SelectedCategory { get; set; }
        public ActivityContactDetail ActivityContactDetails { get; set; }
       
        [DataType(DataType.MultilineText)] 
        public new string FullDescription { get; set; }
        [DataType(DataType.MultilineText)]
        public new string Price { get; set; }
        [DataType(DataType.MultilineText)]
        [Required]
        public new string EligibilityDescription { get; set; }

        public ServiceDetailModel()
        {
            States = new List<State>();
            Suburbs = new List<Suburb>();
            Categories = new List<v_CategoryExplorer>();
             
            
        }
    }
}