using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ISD.BF;
using ISD.DA;
using ISD.Util;

namespace ISD.Provider.Web.API
{
    public class ChartController : ApiController
    {

        static DataAccessComponent dac = new DataAccessComponent();

        public IEnumerable<VisitorRecords> GetAllChartData()
        {
            //var ChartData = dac.getvisitor();
            List<VisitorRecords> ChartData = new List<VisitorRecords>();
            ChartData.Add(new VisitorRecords { DateTime = DateTime.Now, Count = 1 });

            return ChartData;

        }

        public dynamic GetProviderMonthlyVisitorCount(Guid ProviderID)
        {
            int VisitCount = 0;
            var data = new BusinessFunctionComponent().RetrieveProviderVisitorMontlyCount(ProviderID, out VisitCount);


            return new { label = "Visitors", Visitor = VisitCount, data = data.Select(x => new long[] { ((x.DateTime.ToUniversalTime().Ticks - SystemConstants.DatetimeMinTimeTicks) / 10000), x.Count }) };


        }
        public dynamic GetProviderMonthlyVisitorCount(Guid ProviderID, DateTime From, DateTime To)
        {
            int VisitCount = 0;
            var data = new BusinessFunctionComponent().RetrieveProviderVisitorMontlyCount(ProviderID, From, To, out VisitCount);


            return new { label = "Visitors", data = data.Select(x => new long[] { ((x.DateTime.ToUniversalTime().Ticks - SystemConstants.DatetimeMinTimeTicks) / 10000), x.Count }) };
        }

        //public string AddUser(ChartData user)
        //{
        //    var response = dac.AddUser(user);
        //    return response;
        //}
        //public void DeleteUser(int id)
        //{
        //    dac.DeleteUser(id);
        //}
    }
}