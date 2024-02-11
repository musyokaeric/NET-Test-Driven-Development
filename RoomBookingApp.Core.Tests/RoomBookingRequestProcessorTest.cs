using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;
using RoomBookingApp.Core.Enums;

namespace RoomBookingApp.Core
{
    public class RoomBookingRequestProcessorTest
    {
        private RoomBookingRequestProcessor processor;
        private RoomBookingRequest request;
        private Mock<IRoomBookingService> roomBookingServiceMock;
        private List<Room> availableRooms;

        public RoomBookingRequestProcessorTest()
        {
            request = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "test@request.com",
                Date = new DateTime(2021, 10, 20)
            };

            availableRooms = new List<Room> { new Room() { Id = 1 } };

            roomBookingServiceMock = new Mock<IRoomBookingService>();
            roomBookingServiceMock.Setup(r => r.GetAvailableRooms(request.Date))
                .Returns(availableRooms);
            processor = new RoomBookingRequestProcessor(roomBookingServiceMock.Object);
        }

        [Fact]
        public void Should_Return_Room_Booking_Response_With_Request_Values()
        {

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
            Assert.Throws<ArgumentNullException>(() => processor.BookRoom(null));

            var exception = Should.Throw<ArgumentNullException>(() => processor.BookRoom(null));
            exception.ParamName.ShouldBe("bookingRequest");
        }

        [Fact]
        public void Should_Save_Room_Booking_Request()
        {
            RoomBooking savedBooking = null;
            roomBookingServiceMock.Setup(rbs => rbs.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(booking =>
                {
                    savedBooking = booking;
                });

            processor.BookRoom(request);

            roomBookingServiceMock.Verify(rbs => rbs.Save(It.IsAny<RoomBooking>()), Times.Once);

            Assert.NotNull(savedBooking);
            Assert.Equal(savedBooking.FullName, request.FullName);
            Assert.Equal(savedBooking.Email, request.Email);
            Assert.Equal(savedBooking.Date, request.Date);

            savedBooking.ShouldNotBeNull();
            savedBooking.FullName.ShouldBe(request.FullName);
            savedBooking.Email.ShouldBe(request.Email);
            savedBooking.Date.ShouldBe(request.Date);
            savedBooking.RoomId.ShouldBe(availableRooms.First().Id);
        }

        [Fact]
        public void Should_Not_Save_Room_Booking_Request_If_None_Available()
        {
            availableRooms.Clear();

            processor.BookRoom(request);

            roomBookingServiceMock.Verify(rbs => rbs.Save(It.IsAny<RoomBooking>()), Times.Never);
        }

        // Data driven tests
        [Theory]
        [InlineData(BookingResultFlag.Failure, false)]
        [InlineData(BookingResultFlag.Success, true)]
        public void Should_Return_SuccessOrFailure_Flag_In_Result(BookingResultFlag bookingSuccessFlag, bool isAvailable)
        {
            if(!isAvailable)
            {
                availableRooms.Clear();
            }

            var result = processor.BookRoom(request);

            bookingSuccessFlag.ShouldBe(result.Flag); 
        }

        // Data driven tests
        [Theory]
        [InlineData(1, true)]
        [InlineData(null, false)]
        public void Should_Return_RoomBookingId_In_Result(int? roomBookingId, bool isAvailable)
        {
            if (!isAvailable)
            {
                availableRooms.Clear();
            }

            roomBookingServiceMock.Setup(rbs => rbs.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(booking =>
                {
                    booking.Id = roomBookingId.Value;
                });

            var result = processor.BookRoom(request);
            result.RoomBookingId.ShouldBe(roomBookingId);
        }
    }
}
