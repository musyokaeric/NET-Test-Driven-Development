using Microsoft.AspNetCore.Mvc;
using RoomBookingApp.API.Controllers;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.API.Tests
{
    public class RoomBookingControllerTest
    {
        private Mock<IRoomBookingRequestProcessor> roomBookingProcessor;
        private RoomBookingController controller;
        private RoomBookingRequest request;
        private RoomBookingResult result;

        public RoomBookingControllerTest()
        {
            roomBookingProcessor = new Mock<IRoomBookingRequestProcessor>();
            controller = new RoomBookingController(roomBookingProcessor.Object);
            request = new RoomBookingRequest();
            result = new RoomBookingResult();

            roomBookingProcessor.Setup(x=>x.BookRoom(request)).Returns(result);
        }

        [Theory]
        [InlineData(1, true, typeof(OkObjectResult), BookingResultFlag.Success)]
        [InlineData(0, false, typeof(BadRequestObjectResult), BookingResultFlag.Failure)]
        public async void Should_Call_Booking_Method_When_Called
            (int expectedMethodCalls, bool isModeValid, Type expectedActionResultType, BookingResultFlag bookingResultFlag)
        {
            // Arrange
            if (!isModeValid)
                controller.ModelState.AddModelError("key", "ErrorMessage");
            
            this.result.Flag = bookingResultFlag;

            // Act
            var result = await controller.BookRoom(request);

            // Assert
            result.ShouldBeOfType(expectedActionResultType);
            roomBookingProcessor.Verify(x => x.BookRoom(request), Times.Exactly(expectedMethodCalls));
        }
    }
}
