using System;
using System.Threading.Tasks;
using DevSumScheduler.Data;
using Microsoft.AspNetCore.Mvc;

namespace DevSumScheduler.WebApp.Controllers
{
    [Route("api")]
    public class ApiController : Controller
    {
        private readonly DayDataService _dayDataService;
        private readonly SpeakerDataService _speakerDataService;

        public ApiController(DayDataService dayDataService, SpeakerDataService speakerDataService)
        {
            _dayDataService = dayDataService ?? throw new ArgumentNullException(nameof(dayDataService));
            _speakerDataService = speakerDataService ?? throw new ArgumentNullException(nameof(speakerDataService));
        }

        [Produces("application/json")]
        [Route("day/{index}")]
        public async Task<IActionResult> Day(int index)
        {
            var day = await _dayDataService.GetDay(index);

            var json = day != null ? new {day.Title, day.Locations, day.Timeslots} : null;
            return json != null ? Json(json) : (IActionResult) NotFound();
        }
        
        [Produces("text/html")]
        [Route("speaker/{slug}")]
        public async Task<IActionResult> Speaker(string slug)
        {
            var data = await _speakerDataService.GetSpeaker(slug);

            return data != null ? Content(data, "text/html") : (IActionResult) NotFound();
        }
    }
}