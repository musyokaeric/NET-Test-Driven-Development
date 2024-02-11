using Microsoft.Extensions.Logging;
using RoomBookingApp.API.Controllers;

namespace RoomBookingApp.API.Tests
{
    public class WeatherControllerTest
    {
        [Fact]
        public void Should_Return_Forecast_Results()
        {
            var loggerMock = new Mock<ILogger<WeatherForecastController>>();
            var controller = new WeatherForecastController(loggerMock.Object);

            var result = controller.Get();

            Assert.NotNull(result);
            result.Count().ShouldBeGreaterThan(1);
        }
    }
}
