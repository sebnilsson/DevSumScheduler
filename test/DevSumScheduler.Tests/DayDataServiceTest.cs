using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevSumScheduler.Data;
using DevSumScheduler.Tests.Data;
using Xunit;

namespace DevSumScheduler.Tests
{
    public class DayDataServiceTest
    {
        [Theory]
        [MemberData(nameof(GetDay1AndDay2))]
        public async Task GetResult_WithDay1AndDay2_ReturnLocations(string dayData)
        {
            // Arrange
            var service = GetDayDataService(dayData);

            // Act
            var result = await service.GetResult();

            // Assert
            var day = result.Days.First();
            var location1 = day.Locations[0];
            var location2 = day.Locations[1];
            var location3 = day.Locations[2];
            var location4 = day.Locations[3];
            var location5 = day.Locations[4];
            var location6 = day.Locations[5];

            Assert.Equal("Room A", location1);
            Assert.Equal("Room B", location2);
            Assert.Equal("Room C", location3);
            Assert.Equal("Room 202", location4);
            Assert.Equal("Room 300", location5);
            Assert.Equal("Room 307", location6);
        }

        private static DayDataService GetDayDataService(params string[] dayData)
        {
            var dataProvider = new StringDataProvider(dayData);

            return new DayDataService(dataProvider);
        }
        
        public static IEnumerable<object[]> GetDay1AndDay2()
        {
            yield return new object[] {HtmlResources.Day1};
            yield return new object[] {HtmlResources.Day2};
        }

        [Fact]
        public async Task GetResult_WithDay1_ReturnDay1Title()
        {
            // Arrange
            var service = GetDayDataService(HtmlResources.Day1);

            // Act
            var result = await service.GetResult();

            // Assert
            var day = result.Days.First();

            Assert.Equal("Day 1 - Thursday the 31th", day.Title);
        }

        [Fact]
        public async Task GetResult_WithDay1_ReturnExpectedSessionsCount()
        {
            // Arrange
            var service = GetDayDataService(HtmlResources.Day1);

            // Act
            var result = await service.GetResult();

            // Assert
            var day = result.Days.First();

            Assert.Equal(66, day.Sessions.Count);
        }

        [Fact]
        public async Task GetResult_WithDay1_ReturnExpectedTimeslotCount()
        {
            // Arrange
            var service = GetDayDataService(HtmlResources.Day1);

            // Act
            var result = await service.GetResult();

            // Assert
            var day = result.Days.First();

            Assert.Equal(12, day.Timeslots.Count);
        }

        [Fact]
        public async Task GetResult_WithDay1_ReturnFirstSessionsAreNotSelectable()
        {
            // Arrange
            var service = GetDayDataService(HtmlResources.Day1);

            // Act
            var result = await service.GetResult();

            // Assert
            var day = result.Days.First();
            var firstTimeslotsAreaNotSelectable =
                day.Timeslots.Any() && day.Timeslots.Take(3).All(x => !x.IsSelectable);

            Assert.True(firstTimeslotsAreaNotSelectable);
        }

        [Fact]
        public async Task GetResult_WithDay1_ReturnSpotCheckValues()
        {
            // Arrange
            var service = GetDayDataService(HtmlResources.Day1);

            // Act
            var result = await service.GetResult();

            // Assert
            var day = result.Days.First();
            var timeslot1130 = day.Timeslots[4];
            var timeslot1520 = day.Timeslots[9];
            var session1130First = timeslot1130.Sessions[0];
            var session1520Fourth = timeslot1520.Sessions[3];

            Assert.Equal("An Opinionated Approach to ASP.NET Core", session1130First.Title);
            Assert.Equal("Scott Allen", session1130First.SpeakerTitle);
            Assert.Equal("scott-allen", session1130First.SpeakerSlug);
            Assert.Equal(new TimeSpan(11, 30, 0), session1130First.StartsAt);
            Assert.Equal(new TimeSpan(12, 20, 0), session1130First.EndsAt);

            Assert.Equal("Introduction to webpacks", session1520Fourth.Title);
            Assert.Equal("Chris Klug", session1520Fourth.SpeakerTitle);
            Assert.Equal("chris-klug", session1520Fourth.SpeakerSlug);
            Assert.Equal(new TimeSpan(15, 20, 0), session1520Fourth.StartsAt);
            Assert.Equal(new TimeSpan(16, 10, 0), session1520Fourth.EndsAt);
        }

        [Fact]
        public async Task GetResult_WithDay1_ReturnTimeslotsWithExpectedSessionCount()
        {
            // Arrange
            var service = GetDayDataService(HtmlResources.Day1);

            // Act
            var result = await service.GetResult();

            // Assert
            var day = result.Days.First();
            var allTimeslotHasExpectedSessionCount =
                day.Timeslots.Any() && day.Timeslots.All(x => x.Sessions.Count <= 6);

            Assert.True(allTimeslotHasExpectedSessionCount);
        }

        [Fact]
        public async Task GetResult_WithDay2_ReturnDayTitle()
        {
            // Arrange
            var service = GetDayDataService(HtmlResources.Day2);

            // Act
            var result = await service.GetResult();

            // Assert
            var day = result.Days.First();

            Assert.Equal("Day 2 - Friday the 1st", day.Title);
        }

        [Fact]
        public async Task GetResult_WithDay2_ReturnSpotCheckValues()
        {
            // Arrange
            var service = GetDayDataService(HtmlResources.Day2);

            // Act
            var result = await service.GetResult();

            // Assert
            var day2 = result.Days.First();
            var timeslot1130 = day2.Timeslots[3];
            var timeslot1520 = day2.Timeslots[6];
            var session1130First = timeslot1130.Sessions[0];
            var session1410Fifth = timeslot1520.Sessions[4];
            
            Assert.Equal("Indexes - the Unsung heroes of SQL Server", session1130First.Title);
            Assert.Equal("Pinal Dave", session1130First.SpeakerTitle);
            Assert.Equal("pinal-dave", session1130First.SpeakerSlug);
            Assert.Equal(new TimeSpan(11, 30, 0), session1130First.StartsAt);
            Assert.Equal(new TimeSpan(12, 20, 0), session1130First.EndsAt);

            Assert.Equal("How we built one of Sweden’s largest websites in work & education", session1410Fifth.Title);
            Assert.Equal("Peter Örneholm + Lovisa Åblad", session1410Fifth.SpeakerTitle);
            Assert.Equal("lovisa-ablad", session1410Fifth.SpeakerSlug);
            Assert.Equal(new TimeSpan(14, 10, 0), session1410Fifth.StartsAt);
            Assert.Equal(new TimeSpan(15, 0, 0), session1410Fifth.EndsAt);
        }
    }
}