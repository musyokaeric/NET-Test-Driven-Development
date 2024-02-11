using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.Core
{
    public class RoomBookingRequestProcessorTest
    {
        private RoomBookingRequestProcessor processor;

        public RoomBookingRequestProcessorTest()
        {
            processor = new RoomBookingRequestProcessor();
        }

        [Fact]
        public void Should_Return_Room_Booking_Response_With_Request_Values()
        {
            // Arrange
            var request = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "test@request.com",
                Date = new DateTime(2021, 10, 20)
            };
            processor = new RoomBookingRequestProcessor();

            // Act
            RoomBookingResult result = processor.BookRoom(request);

            // Assert (xunit default)
            Assert.NotNull(result);
            Assert.Equal(request.FullName, result.FullName);
            Assert.Equal(request.Email, result.Email);
            Assert.Equal(request.Date, result.Date);

            // Assert (shouldly)
            result.ShouldNotBeNull();
            result.FullName.ShouldBe(request.FullName);
            result.Email.ShouldBe(request.Email);
            result.Date.ShouldBe(request.Date);
        }

        [Fact]
        public void Should_Throw_Exception_For_Null_Request()
        {
            processor = new RoomBookingRequestProcessor();

            Assert.Throws<ArgumentNullException>(() => processor.BookRoom(null));

            var exception = Should.Throw<ArgumentNullException>(()=> processor.BookRoom(null));
            exception.ParamName.ShouldBe("bookingRequest");
        }
    }
}
