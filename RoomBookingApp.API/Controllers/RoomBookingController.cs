using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomBookingController : ControllerBase
    {
        private IRoomBookingRequestProcessor roomBookingProcessor;

        public RoomBookingController(IRoomBookingRequestProcessor roomBookingProcessor)
        {
            this.roomBookingProcessor = roomBookingProcessor;
        }

        [HttpPost]
        public async Task<IActionResult> BookRoom(RoomBookingRequest request)
        {
            if (ModelState.IsValid)
            {
                var result = roomBookingProcessor.BookRoom(request);

                if (result.Flag == BookingResultFlag.Success) return Ok(result);

                ModelState.AddModelError(nameof(RoomBookingRequest.Date), "No rooms available for given date");
            }

            return BadRequest(ModelState);
        }
    }
}
