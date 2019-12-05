using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Zach_BugTracker.Models;

namespace Zach_BugTracker.Controllers
{


    [Authorize]
    [RequireHttps]

    public class GraphingController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Graphing
        public JsonResult ProduceChart1Data()
        {
            var myData = new List<MorrisBarData>();
            MorrisBarData data = null;
            foreach(var priority in db.TicketPriorities.ToList())
            {
                data = new MorrisBarData();
                data.label = priority.PriorityName;
                data.value = db.Tickets.Where(t => t.TicketPriority.PriorityName == priority.PriorityName).Count();
                myData.Add(data);
            }
            return Json(myData);
        }


        public JsonResult ProduceChart2Data()
        {
            var myData = new List<MorrisBarData>();
            MorrisBarData data = null;
            foreach (var status in db.TicketStatus.ToList())
            {
                myData.Add(new MorrisBarData
                {
                    label = status.StatusName,
                    value = db.Tickets.Where(t => t.TicketStatus.StatusName == status.StatusName).Count()
                });
            }
            return Json(myData);
        }


        public JsonResult ProduceChart3Data()
        {
            var myData = new List<MorrisBarData>();
            MorrisBarData data = null;
            foreach (var type in db.TicketTypes.ToList())
            {
                myData.Add(new MorrisBarData
                {
                    label = type.TypeName,
                    value = db.Tickets.Where(t => t.TicketType.TypeName == type.TypeName).Count()
                });
            }
            return Json(myData);
        }


        //public JsonResult ProduceChart4Data()
        //{
        //    var myData = new List<MorrisBarData>();
        //    MorrisBarData data = null;
        //    foreach (var status in db.TicketStatus.ToList())
        //    {
        //        myData.Add(new MorrisBarData
        //        {
        //            label = status.StatusName,
        //            value = db.Tickets.Where(t => t.TicketStatus.StatusName == status.StatusName).Count()
        //        });
        //    }
        //    return Json(myData);
        //}

    }
}