using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RoomBookingApp.Core.Processors
{
    public class RoomBookingRequestProcessor
    {
        private readonly IRoomBookingService roomBookingService;

        public RoomBookingRequestProcessor(IRoomBookingService roomBookingService)
        {
            this.roomBookingService = roomBookingService;
        }

        public RoomBookingResult BookRoom(RoomBookingRequest bookingRequest)
        {
            if (bookingRequest == null) throw new ArgumentNullException(nameof(bookingRequest));

            var availableRooms = roomBookingService.GetAvailableRooms(bookingRequest.Date);

            if(availableRooms.Any())
                roomBookingService.Save(CreateRoomBookingObject<RoomBooking>(bookingRequest));

            return CreateRoomBookingObject<RoomBookingResult>(bookingRequest);
        }

        private TRoomBooking CreateRoomBookingObject<TRoomBooking>(RoomBookingRequest bookingRequest)
            where TRoomBooking : RoomBookingBase, new()
        {
            return new TRoomBooking
            {
                FullName = bookingRequest.FullName,
                Email = bookingRequest.Email,
                Date = bookingRequest.Date,
            };
        }
    }
}