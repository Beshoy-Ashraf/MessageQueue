using FormulaAirLine.API.Models;
using FormulaAirLine.API.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace FormulaAirLine.API.Controller;


[ApiController]
[Route("api/[controller]")]
public class BookingController(ILogger<BookingController> logger, IMessageProducer messageProducer) : ControllerBase
{


      public static readonly List<Booking> Bookings = [];
      [HttpPost]
      public async Task<IActionResult> CreateBooking([FromBody] Booking booking)
      {
            logger.LogInformation("Received booking request for {PassengerName} {PassengerSurname}", booking.PassengerName, booking.PassengerSurname);

            Bookings.Add(booking);

            await messageProducer.SendMessage<Booking>(booking);

            logger.LogInformation("Booking request for {PassengerName} {PassengerSurname} sent to the queue", booking.PassengerName, booking.PassengerSurname);

            return Ok(new { Message = "Booking request received and sent to the queue." });
      }

}
