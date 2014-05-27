using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISD.Data.EDM;
using ISD.Util;

namespace ISD.Application.provider.MVC.Models
{
    public class ServiceDetailModel :ISD.Data.EDM.v_ActivityExplorer
    {
        public SystemConstants.FormMode Mode { get; set; }
        public IEnumerable<ActivityImageDetail> Images { get; set; }
        public IEnumerable<v_ActivityExplorer> Services { get; set; }
        public IEnumerable<v_ActivityClinicianExplorer> Clinicians { get; set; }

        public IEnumerable<State> States { get; set; }
        public IEnumerable<Suburb> Suburbs { get; set; }
        public ActivityContactDetail ActivityContactDetails { get; set; }

    }
}