using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using ISD.Data.EDM;
using ISD.Util;

namespace ISD.Application.provider.MVC.Models
{
    public class CliniciansModels : Clinicians
    {
        [Required]

        public string ClinicianName { get; set; }

        [Required]
        [EmailAddress]
        public string ClinicianEmail { get; set; }

        [Required]
        public int ClinicianType { get; set; }

        public ClinicianTimetable Timetable { get; set; }

        public int RecurEvery { get; set; }
        public bool OnMonday { get; set; }
        public bool OnTuesday { get; set; }
        public bool OnWednesday { get; set; }
        public bool OnThursday { get; set; }
        public bool OnFriday { get; set; }
        public bool OnSaturday { get; set; }
        public bool OnSunday { get; set; }







        public CliniciansModels()
        {
            Timetable = new ClinicianTimetable();
 
            Timetable.RecurEvery = RecurEvery;
            Timetable.OnMonday = OnMonday;
            Timetable.OnTuesday = OnTuesday;
            Timetable.OnWednesday = OnWednesday;
            Timetable.OnThursday = OnThursday;
            Timetable.OnFriday = OnFriday;
            Timetable.OnSaturday = OnSaturday;
            Timetable.OnSunday = OnSunday;

            Name = ClinicianName="";
            Email = ClinicianEmail = "";
            Type = ClinicianType;
        }

        
        public List<ListItem> clinicianTypes =
           new List<ListItem>
            {
                new ListItem {Value = "", Text = "Select Type"},
                new ListItem {Value = ((int)SystemConstants.CliniciansType.Public).ToString(), Text = "Public"},
                new ListItem {Value = ((int)SystemConstants.CliniciansType.Private).ToString(), Text = "Private"}
            };
    }
}

