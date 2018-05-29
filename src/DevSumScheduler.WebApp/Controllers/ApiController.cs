using System;
using System.Globalization;
using System.Linq;
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
        [Route("days")]
        public async Task<IActionResult> Days()
        {
            var result = await _dayDataService.GetResult();
            if (result == null)
                return NotFound();

            var data = result.Days.Select(day => new {day.Title});
            return Json(data);
        }

        [Produces("application/json")]
        [Route("day/{index}")]
        public async Task<IActionResult> Day(int index)
        {
            var day = await _dayDataService.GetDay(index);

            var json = day != null
                ? GetDayJson(day)
                : null;
            return json != null ? Json(json) : (IActionResult) NotFound();
        }

        [Produces("text/html")]
        [Route("speaker/{slug}")]
        public async Task<IActionResult> Speaker(string slug)
        {
            var data = await _speakerDataService.GetSpeaker(slug);

            return data != null ? Content(data, "text/html") : (IActionResult) NotFound();
        }

        private static object GetDayJson(Day day)
        {
            return new
            {
                day.Title,
                day.Locations,
                Timeslots = day.Timeslots.Select(t => new
                {
                    EndsAt = FormatTimeSpan(t.EndsAt),
                    t.IsSelectable,
                    Sessions = t.Sessions.Select(s => new
                    {
                        EndsAt = FormatTimeSpan(s.EndsAt),
                        s.Location,
                        s.SpeakerSlug,
                        s.SpeakerTitle,
                        StartsAt = FormatTimeSpan(s.StartsAt),
                        s.Title
                    }).ToList(),
                    StartsAt = FormatTimeSpan(t.StartsAt)
                }).ToList()
            };

            string FormatTimeSpan(TimeSpan timeSpan)
            {
                return timeSpan.ToString(@"hh\:mm", DateTimeFormatInfo.InvariantInfo);
            }
        }
    }
}