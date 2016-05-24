using System.Web.Mvc;

namespace DevSumScheduler.Controllers
{
    [Route("{action=index}", Name = "Home")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var scheduleTables = ScheduleService.GetScheduleTables();
            if (scheduleTables == null)
            {
                return this.View("NoData");
            }

            return this.View(scheduleTables);
        }

        public ActionResult About()
        {
            this.ViewBag.Title = "About";

            return this.View();
        }
    }
}