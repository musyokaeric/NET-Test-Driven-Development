﻿using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Domain;

namespace RoomBookingApp.Persistence.Repositories
{
    public class RoomBookingService : IRoomBookingService
    {
        private readonly RoomBookingAppDbContext context;

        public RoomBookingService(RoomBookingAppDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<Room> GetAvailableRooms(DateTime date)
        {
            return context.Rooms.Where(r => !r.RoomBookings.Any(r => r.Date == date)).ToList();
        }

        public void Save(RoomBooking roomBooking)
        {
            context.RoomBookings.Add(roomBooking);
            context.SaveChanges();
        }
    }
}
