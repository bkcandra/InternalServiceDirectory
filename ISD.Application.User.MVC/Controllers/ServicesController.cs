using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using ISD.Data.EDM;
using ISD.Util;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using nullpointer.Metaphone;
using ISD.BF;
using PagedList;

namespace ISD.Application.User.MVC.Controllers
{
    public class ServicesController : Controller
    {
        private ISDEntities db = new ISDEntities();
        // GET: Services

        public async Task<ActionResult> index()
        {
            return View();
        }


        public async Task<ActionResult> Paged(int? page, int? pageSize, string category, string clinic, string search)
        {
            bool MonFilter, TueFilter, WedFilter, ThursFilter, FriFilter, SatFilter, SunFilter = false;

            if (!string.IsNullOrEmpty(search))
            {
                List<String> parameters = new BusinessFunctionComponent().RefineSearchKey(search);

                foreach (var parameter in parameters)
                {
                    if (!string.IsNullOrEmpty(parameter))
                    {
                        if (parameter.StartsWith(SystemConstants.Query))
                        {
                            search = parameter.Replace(SystemConstants.Query, string.Empty);
                        }
                        else if (parameter.StartsWith(SystemConstants.Location))
                        {
                            clinic = parameter.Replace(SystemConstants.Location, string.Empty).ToUpper();
                        }
                        else if (parameter.StartsWith(SystemConstants.Day))
                        {
                            string[] days = parameter.Replace(SystemConstants.Day, string.Empty).Split(';');
                            foreach (var day in days)
                            {
                                if (day.ToUpper().Equals(DayOfWeek.Monday.ToString().ToUpper()))
                                    MonFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Tuesday.ToString().ToUpper()))
                                    TueFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Wednesday.ToString().ToUpper()))
                                    WedFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Thursday.ToString().ToUpper()))
                                    ThursFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Friday.ToString().ToUpper()))
                                    FriFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Saturday.ToString().ToUpper()))
                                    SatFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Sunday.ToString().ToUpper()))
                                    SunFilter = true;
                            }
                        }
                    }
                }
            }
            var services = db.v_ActivityExplorer.AsEnumerable();
            HashSet<int> serviceIDs = new HashSet<int>(services.Select(x => x.ID));
            if (!string.IsNullOrEmpty(category))
            {
                var cats = category.Split(',').ToList();
                services = services.Where(x => (cats.Contains(x.CategoryID.ToString()) || cats.Contains(x.CategoryLevel1ParentID.ToString())));
            }
            if (!string.IsNullOrEmpty(clinic) && clinic != "0")
            {
                List<int> clinicsQ = clinic.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                var Clinicianinlocs = (await db.ClinicianTimetable.ToListAsync()).Where(x => clinicsQ.Contains(x.LocationID.Value)).Select(x => x.ClinicianID);
                var ActClinicianList = (await db.ActivityClinician.ToListAsync()).Where(c => Clinicianinlocs.Contains(c.ClinicianID.Value)).Select(c => c.ActivityID);
                services = services.Where(x => ActClinicianList.Contains(x.ID));
            }

            if (!string.IsNullOrEmpty(search))
            {
                Dictionary<string, HashSet<int>> ActDictionary = new Dictionary<string, HashSet<int>>();
                foreach (var service in services)
                {
                    //adding title to dictionary
                    service.Name = Regex.Replace(service.Name, @"[!@#-;,:$%_]", "");
                    service.ShortDescription = Regex.Replace(service.ShortDescription, @"[!@#-;,:$%_]", "");

                    string[] actTitle = service.Name.Trim().Split();
                    foreach (var word in actTitle)
                    {
                        DoubleMetaphone mp = new DoubleMetaphone(word);
                        HashSet<int> savedAct = new HashSet<int>();
                        ActDictionary.TryGetValue(mp.PrimaryKey, out savedAct);

                        if (savedAct == null)
                        {
                            savedAct = new HashSet<int>();
                            ActDictionary[mp.PrimaryKey] = savedAct;
                        }
                        savedAct.Add(service.ID);

                        if (mp.AlternateKey != null)
                        {
                            savedAct = new HashSet<int>();
                            ActDictionary.TryGetValue(mp.AlternateKey, out savedAct);

                            if (savedAct == null)
                            {
                                savedAct = new HashSet<int>();
                                ActDictionary[mp.AlternateKey] = savedAct;
                            }
                            savedAct.Add(service.ID);
                        }
                    }
                    //adding keyword to dictionary if not null
                    if (!string.IsNullOrEmpty(service.Keywords))
                    {
                        string[] keywords = service.Keywords.Trim().Split(';');
                        foreach (var word in keywords)
                        {
                            DoubleMetaphone mp = new DoubleMetaphone(word);
                            HashSet<int> savedAct = new HashSet<int>();
                            ActDictionary.TryGetValue(mp.PrimaryKey, out savedAct);

                            if (savedAct == null)
                            {
                                savedAct = new HashSet<int>();
                                ActDictionary[mp.PrimaryKey] = savedAct;
                            }
                            savedAct.Add(service.ID);

                            if (mp.AlternateKey != null)
                            {
                                savedAct = new HashSet<int>();
                                ActDictionary.TryGetValue(mp.AlternateKey, out savedAct);

                                if (savedAct == null)
                                {
                                    savedAct = new HashSet<int>();
                                    ActDictionary[mp.AlternateKey] = savedAct;
                                }
                                savedAct.Add(service.ID);
                            }
                        }
                    }
                }

                String[] searchQ = Array.ConvertAll(search.Split(), c => c.Trim());
                HashSet<string> keywordsMPs = new HashSet<string>();
                foreach (var word in searchQ)
                {
                    DoubleMetaphone mp = new DoubleMetaphone(word);
                    if (mp.PrimaryKey != null)
                        keywordsMPs.Add(mp.PrimaryKey);
                    if (mp.AlternateKey != null)
                        keywordsMPs.Add(mp.AlternateKey);
                }
                HashSet<int> matchesAct = new HashSet<int>();
                foreach (var keywordsMP in keywordsMPs)
                {
                    HashSet<int> matches = new HashSet<int>();
                    ActDictionary.TryGetValue(keywordsMP, out matches);
                    if (matches != null)
                    {
                        matchesAct.UnionWith(matches);
                    }
                }
                services = services.Where(x => matchesAct.Contains(x.ID));
            }


            // paging
            //end paging
            var results = services.OrderBy(x => x.Name).Skip((page ?? 0) * (pageSize ?? 50)).Take(pageSize ?? 50);
            // if no page was specified in the querystring, default to the first page (1)
            var pageNumber = page ?? 1;
            var Pagedresults = services.OrderBy(x => x.ID).ToPagedList(pageNumber, 1); // will only contain 25 products max because of the pageSize
            ViewBag.Services = Pagedresults;
            return View(Pagedresults);
        }

        public async Task<ActionResult> GetServices(int? page, int? pageSize, string category, string clinic, string search)
        {
            bool MonFilter, TueFilter, WedFilter, ThursFilter, FriFilter, SatFilter, SunFilter = false;

            if (!string.IsNullOrEmpty(search))
            {
                List<String> parameters = new BusinessFunctionComponent().RefineSearchKey(search);

                foreach (var parameter in parameters)
                {
                    if (!string.IsNullOrEmpty(parameter))
                    {
                        if (parameter.StartsWith(SystemConstants.Query))
                        {
                            search = parameter.Replace(SystemConstants.Query, string.Empty);
                        }
                        else if (parameter.StartsWith(SystemConstants.Location))
                        {
                            clinic = parameter.Replace(SystemConstants.Location, string.Empty).ToUpper();
                        }
                        else if (parameter.StartsWith(SystemConstants.Day))
                        {
                            string[] days = parameter.Replace(SystemConstants.Day, string.Empty).Split(';');
                            foreach (var day in days)
                            {
                                if (day.ToUpper().Equals(DayOfWeek.Monday.ToString().ToUpper()))
                                    MonFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Tuesday.ToString().ToUpper()))
                                    TueFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Wednesday.ToString().ToUpper()))
                                    WedFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Thursday.ToString().ToUpper()))
                                    ThursFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Friday.ToString().ToUpper()))
                                    FriFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Saturday.ToString().ToUpper()))
                                    SatFilter = true;
                                if (day.ToUpper().Equals(DayOfWeek.Sunday.ToString().ToUpper()))
                                    SunFilter = true;
                            }
                        }
                    }
                }
            }
            var services = db.v_ActivityExplorer.AsEnumerable();
            HashSet<int> serviceIDs = new HashSet<int>(services.Select(x => x.ID));
            if (!string.IsNullOrEmpty(category))
            {
                var cats = category.Split(',').ToList();
                services = services.Where(x => (cats.Contains(x.CategoryID.ToString()) || cats.Contains(x.CategoryLevel1ParentID.ToString())));
            }
            if (!string.IsNullOrEmpty(clinic) && clinic != "0")
            {
                List<int> clinicsQ = clinic.Split(',').Select(x=> Convert.ToInt32(x)).ToList();
                var Clinicianinlocs = (await db.ClinicianTimetable.ToListAsync()).Where(x => clinicsQ.Contains(x.LocationID.Value)).Select(x => x.ClinicianID);
                var ActClinicianList = (await db.ActivityClinician.ToListAsync()).Where(c => Clinicianinlocs.Contains(c.ClinicianID.Value)).Select(c => c.ActivityID);
                services = services.Where(x => ActClinicianList.Contains(x.ID));
            }

            if (!string.IsNullOrEmpty(search))
            {
                Dictionary<string, HashSet<int>> ActDictionary = new Dictionary<string, HashSet<int>>();
                foreach (var service in services)
                {
                    //adding title to dictionary
                    service.Name = Regex.Replace(service.Name, @"[!@#-;,:$%_]", "");
                    service.ShortDescription = Regex.Replace(service.ShortDescription, @"[!@#-;,:$%_]", "");

                    string[] actTitle = service.Name.Trim().Split();
                    foreach (var word in actTitle)
                    {
                        DoubleMetaphone mp = new DoubleMetaphone(word);
                        HashSet<int> savedAct = new HashSet<int>();
                        ActDictionary.TryGetValue(mp.PrimaryKey, out savedAct);

                        if (savedAct == null)
                        {
                            savedAct = new HashSet<int>();
                            ActDictionary[mp.PrimaryKey] = savedAct;
                        }
                        savedAct.Add(service.ID);

                        if (mp.AlternateKey != null)
                        {
                            savedAct = new HashSet<int>();
                            ActDictionary.TryGetValue(mp.AlternateKey, out savedAct);

                            if (savedAct == null)
                            {
                                savedAct = new HashSet<int>();
                                ActDictionary[mp.AlternateKey] = savedAct;
                            }
                            savedAct.Add(service.ID);
                        }
                    }
                    //adding keyword to dictionary if not null
                    if (!string.IsNullOrEmpty(service.Keywords))
                    {
                        string[] keywords = service.Keywords.Trim().Split(';');
                        foreach (var word in keywords)
                        {
                            DoubleMetaphone mp = new DoubleMetaphone(word);
                            HashSet<int> savedAct = new HashSet<int>();
                            ActDictionary.TryGetValue(mp.PrimaryKey, out savedAct);

                            if (savedAct == null)
                            {
                                savedAct = new HashSet<int>();
                                ActDictionary[mp.PrimaryKey] = savedAct;
                            }
                            savedAct.Add(service.ID);

                            if (mp.AlternateKey != null)
                            {
                                savedAct = new HashSet<int>();
                                ActDictionary.TryGetValue(mp.AlternateKey, out savedAct);

                                if (savedAct == null)
                                {
                                    savedAct = new HashSet<int>();
                                    ActDictionary[mp.AlternateKey] = savedAct;
                                }
                                savedAct.Add(service.ID);
                            }
                        }
                    }
                }

                String[] searchQ = Array.ConvertAll(search.Split(), c => c.Trim());
                HashSet<string> keywordsMPs = new HashSet<string>();
                foreach (var word in searchQ)
                {
                    DoubleMetaphone mp = new DoubleMetaphone(word);
                    if (mp.PrimaryKey != null)
                        keywordsMPs.Add(mp.PrimaryKey);
                    if (mp.AlternateKey != null)
                        keywordsMPs.Add(mp.AlternateKey);
                }
                HashSet<int> matchesAct = new HashSet<int>();
                foreach (var keywordsMP in keywordsMPs)
                {
                    HashSet<int> matches = new HashSet<int>();
                    ActDictionary.TryGetValue(keywordsMP, out matches);
                    if (matches != null)
                    {
                        matchesAct.UnionWith(matches);
                    }
                }
                services = services.Where(x => matchesAct.Contains(x.ID));
            }

            
            // paging

            //end paging

            
            //var results = services.OrderBy(x => x.Name).Skip((page ?? 0) * (pageSize ?? 50)).Take(pageSize ?? 50);
            // if no page was specified in the querystring, default to the first page (1)
            var pageNumber = page ?? 1;
            var results = services.OrderBy(x => x.Name).ToPagedList(pageNumber, 1); // will only contain 25 products max because of the pageSize
            return PartialView("_PartialServicesListing", results);

            



        }

        [HttpGet]
        public async Task<JsonResult> GetCategoriesJson(string selected)
        {
            string[] selList = null;
            if (!string.IsNullOrEmpty(selected))
            {
                selList = selected.Split(',');
            }

            var categories = await db.v_CategoryExplorer.ToListAsync();
            List<jstree> data = new List<jstree>();

            foreach (var cat in categories)
            {
                var nodeParent = cat.Level1ParentID.ToString();
                var nodeIcon = "fa fa-asterisk";
                bool stateIsDisabled = false;
                bool stateIsOpened = false;
                bool stateIsSelected = false;

                if (cat.Level1ParentID == 0)
                {
                    nodeParent = "#";
                    nodeIcon = "fa fa-folder";
                }
                if (selList != null)
                    if (selList.Contains(cat.ID.ToString()) || selList.Contains(cat.Level1ParentID.ToString()))
                    {
                        stateIsOpened = true;
                        stateIsSelected = true;
                    }

                data.Add(new jstree
                {
                    id = cat.ID.ToString(),
                    text = cat.Name,
                    parent = nodeParent,
                    icon = nodeIcon,
                    state = new jsstate()
                    {
                        disabled = stateIsDisabled,
                        opened = stateIsOpened,
                        selected = stateIsSelected
                    }
                });
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }

    public class jstree
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public jsstate state { get; set; }

    }

    public class jsstate
    {
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
    }

    public class ServiceQuery
    {
        public List<int> category { get; set; }
        public List<int> clinic { get; set; }
        public string search { get; set; }


    }
}

