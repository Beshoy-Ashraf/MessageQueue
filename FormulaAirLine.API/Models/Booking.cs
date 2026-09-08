namespace FormulaAirLine.API.Models;

public class Booking
{
      public Guid Id { get; set; }
      public string PassengerName { get; set; } = string.Empty;
      public string PassengerSurname { get; set; } = string.Empty;
      public string PassengerEmail { get; set; } = string.Empty;
      public string PassengerPhone { get; set; } = string.Empty;
      public string From { get; set; } = string.Empty;
      public string To { get; set; } = string.Empty;

}
