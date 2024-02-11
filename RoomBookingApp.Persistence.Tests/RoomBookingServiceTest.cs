using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Bson;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistence.Repositories;

namespace RoomBookingApp.Persistence.Tests
{
    public class RoomBookingServiceTest
    {
        private DbContextOptions<RoomBookingAppDbContext> dbOptions;

        public RoomBookingServiceTest()
        {
            
        }

        [Fact]
        public void Should_Return_Available_Rooms()
        {
            // Arrange
            var date = new DateTime(2021, 06, 09);

            dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>().UseInMemoryDatabase("AvailableRoomTest").Options;

            using var context = new RoomBookingAppDbContext(dbOptions);

            context.Add(new Room { Id = 1, Name = "Room 1" });
            context.Add(new Room { Id = 2, Name = "Room 2" });
            context.Add(new Room { Id = 3, Name = "Room 3" });

            context.Add(new RoomBooking { RoomId = 1, Date = date, Email = "firstCustomer@gmail.com", FullName = "First Customer" });
            context.Add(new RoomBooking { RoomId = 2, Date = date.AddDays(-1), Email = "secondCustomer@gmail.com", FullName = "Second Customer" });

            context.SaveChanges();

            var roomBookingService = new RoomBookingService(context);

            // Act
            var availableRooms = roomBookingService.GetAvailableRooms(date);

            // Assert
            Assert.Equal(2, availableRooms.Count());
            Assert.Contains(availableRooms, r => r.Id == 2);
            Assert.Contains(availableRooms, r => r.Id == 3);
            Assert.DoesNotContain(availableRooms, r => r.Id == 1);
        }

        [Fact]
        public void Should_Save_Room_Booking()
        {    
            dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>().UseInMemoryDatabase("ShouldSaveTest").Options;

            var roomBooking = new RoomBooking { RoomId = 1, Date = new DateTime(2021, 06, 09), Email = "firstCustomer@gmail.com", FullName = "First Customer" };

            using var context = new RoomBookingAppDbContext(dbOptions);

            var roomBookingService = new RoomBookingService(context);

            roomBookingService.Save(roomBooking);

            var bookings = context.RoomBookings.ToList();
            var booking = Assert.Single(bookings);

            Assert.Equal(roomBooking.Date, booking.Date);
            Assert.Equal(roomBooking.RoomId, booking.RoomId);
            Assert.IsType<RoomBooking>(booking);
        }
    }
}
