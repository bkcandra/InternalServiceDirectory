using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Backload.Controllers;

namespace ISD.Application.provider.MVC.Controllers
{
    public class BackloadDemoController : Controller
    {
        //
        // GET: /BackupDemo/
        public ActionResult Index()
        {
            return View();
        }

        public class FileUploadDerivedController : BackloadController
        {
            public async Task<ActionResult> FileHandler()
            {
                // Call base class method to handle the file upload asynchronously
                ActionResult result = await base.HandleRequestAsync();
                return result;
            }
        }
    }
}
