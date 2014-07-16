using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ISD.Data.EDM;
using ISD.Util;
using Microsoft.Ajax.Utilities;

namespace ISD.Application.provider.MVC.Models
{
    public class ServiceDetailModel : ISD.Data.EDM.v_ActivityExplorer
    {
        public SystemConstants.FormMode Mode { get; set; }
        public ActivityImage ImageInfo { get; set; }
        public IEnumerable<ActivityImageDetail> Images { get; set; }
        public List<v_ActivityExplorer> Services { get; set; }
        public List<v_ActivityClinicianExplorer> Clinicians { get; set; }
        public List<Clinicians> CliniciansList { get; set; }
        public List<ServiceRequirements> ServiceRequirements { get; set; }
        public List<v_CategoryExplorer> Categories { get; set; }
        [RequiredList(ErrorMessage = "Select at least 1 category from the list")]
        public ICollection<int> SelectedCategory { get; set; }
        [RequiredList(ErrorMessage = "Select at least 1 clinician from the list")]
        public ICollection<int> SelectedClinicians { get; set; }
        [Required]
        public override string Address { get; set; }
        [Required(ErrorMessage = "State must not empty")]
        public override Nullable<int> StateID { get; set; }
        [Required(ErrorMessage = "Suburb must not empty")]
        public override Nullable<int> SuburbID { get; set; }
        [Required(ErrorMessage = "Postcode must not empty")]
        public override Nullable<int> PostCode { get; set; }
        [Required]
        public override string Name { get; set; }
        [Required]
        public override string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public override string Email { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please summarise your service")]
        public override string FullDescription { get; set; }
        [DataType(DataType.MultilineText)]
        public override string Price { get; set; }
        [DataType(DataType.MultilineText)]
        public override string Note { get; set; }
        [DataType(DataType.MultilineText)]
        public override string Assessment { get; set; }
        [DataType(DataType.MultilineText)]
        public override string Keywords { get; set; }
        [DataType(DataType.MultilineText)]        


        public List<ListItem> StatesList =
          new List<ListItem>
            {
                new ListItem {Value = "", Text = "Select State"},
                };
        public List<ListItem> SuburbList =
         new List<ListItem>
            {
                new ListItem {Value = "", Text = "Select Suburb"},
                };


        public ServiceDetailModel()
        {
            Categories = new List<v_CategoryExplorer>();
            SelectedCategory = new List<int>();
            SelectedClinicians = new List<int>();
            StatesList = new List<ListItem>();
            SuburbList = new List<ListItem>();
        }

    }

    public class ServiceRequirements
    {
        public int Type { get; set; }
        public int ValueId { get; set; }
        public int ValueName { get; set; }
    }
    public class ServiceImageDetailModel : ISD.Data.EDM.ActivityImage
    {
        public int ImageInfoID { get; set; }

        public IEnumerable<ActivityImageDetail> Images { get; set; }
    }

    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RequiredList : ValidationAttribute
    {
        private const string defaultError = "'{0}' must have at least one element.";
        public RequiredList()
            : base(defaultError) //
        {
        }

        public override bool IsValid(object value)
        {
            IList list = value as IList;
            return (list != null && list.Count > 0);
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(this.ErrorMessageString, name);
        }
    }
}