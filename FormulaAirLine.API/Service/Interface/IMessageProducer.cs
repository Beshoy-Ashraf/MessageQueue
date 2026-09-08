namespace FormulaAirLine.API.Service.Interface;

public interface IMessageProducer
{
      public Task SendMessage<T>(T message);
}
