namespace Maybe.Domain.Exceptions
{
    public class TelegramException : Exception
    {
        public TelegramException(string message, Exception ex) : base(message, ex)
        { 
        }
    }
}
