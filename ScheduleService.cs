using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Caching;

using DevSumScheduler.ViewModels;

namespace DevSumScheduler
{
    public static class ScheduleService
    {
        private const int CacheExpirationMinutes = 30;

        private const string ScheduleTablesCacheKey = "DevSumScheduler.ScheduleService.GetScheduleTables";

        private static readonly NamedLock CacheLock = new NamedLock();

        private static readonly IEnumerable<string> ScheduleTableUrls = new[]
                                                                            {
                                                                                "http://www.devsum.se/day-1/",
                                                                                "http://www.devsum.se/day-2/"
                                                                            };

        public static ICollection<ScheduleTable> GetScheduleTables()
        {
            var cachedScheduleTables = MemoryCache.Default[ScheduleTablesCacheKey] as ICollection<ScheduleTable>;

            if (cachedScheduleTables == null)
            {
                CacheLock.RunWithLock(
                    ScheduleTablesCacheKey,
                    () =>
                        {
                            cachedScheduleTables =
                                MemoryCache.Default[ScheduleTablesCacheKey] as ICollection<ScheduleTable>;

                            if (cachedScheduleTables == null)
                            {
                                cachedScheduleTables = GetScheduleTablesInternal();

                                bool hasScheduleTablesData = cachedScheduleTables != null && cachedScheduleTables.Any()
                                                             && cachedScheduleTables.SelectMany(x => x.Rows).Any();

                                if (!hasScheduleTablesData)
                                {
                                    return;
                                }

                                MemoryCache.Default.Add(
                                    ScheduleTablesCacheKey,
                                    cachedScheduleTables,
                                    DateTime.Now.AddMinutes(CacheExpirationMinutes));
                            }
                        });
            }

            return cachedScheduleTables;
        }

        private static ICollection<ScheduleTable> GetScheduleTablesInternal()
        {
            try
            {
                var scheduleTables = from url in ScheduleTableUrls.AsParallel()
                                     let html = GetScheduleTableHtml(url)
                                     from table in ScheduleTable.ParseHtml(html)
                                     select table;

                return scheduleTables.ToList();
            }
            catch (Exception ex)
            {
                // TODO: Log Exception

                return null;
            }
        }

        private static string GetScheduleTableHtml(string url)
        {
            using (var client = new HttpClient())
            {
                var responseTask = client.GetAsync(url);
                responseTask.Wait();

                var response = responseTask.Result;

                var contentTask = response.Content.ReadAsStringAsync();
                contentTask.Wait();

                string html = contentTask.Result;
                return html;
            }
        }
    }
}