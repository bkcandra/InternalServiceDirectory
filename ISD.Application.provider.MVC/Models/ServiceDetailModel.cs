using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ISD.Data.EDM;

namespace ISD.Application.provider.MVC.Models
{
    public class ServiceDetailModel :ISD.Data.EDM.v_ActivityExplorer
    {
        public IEnumerable<ActivityImageDetail> Images { get; set; }
        public IEnumerable<v_ActivityExplorer> Services { get; set; }
        public IEnumerable<v_ActivityClinicianExplorer> Clinicians { get; set; }
    }
}