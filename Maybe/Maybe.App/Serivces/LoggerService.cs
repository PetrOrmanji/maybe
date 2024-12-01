using EnsureThat;
using Serilog;

namespace Maybe.App.Serivces
{
    public class LoggerService
    {
        private const string TelegramNotificationError = "Не удалось отправить уведомление в телеграмм";

        private readonly TelegramService _telegramService;

        public LoggerService(TelegramService telegramService) 
        { 
            EnsureArg.IsNotNull(telegramService, nameof(telegramService));

            _telegramService = telegramService;
        }

        public async void Error(Exception ex, string message)
        {
            Log.Logger.Error(ex, message);

            try
            {
                await _telegramService.MakeNotification($"❤️ {message}");
            }
            catch (Exception exсeption)
            {
                Log.Logger.Error(exсeption, TelegramNotificationError);
            }
        }

        public async void Warn(string message)
        {
            Log.Logger.Warning(message);

            try
            {
                await _telegramService.MakeNotification($"💛 {message}");
            }
            catch (Exception exсeption)
            {
                Log.Logger.Error(exсeption, TelegramNotificationError);
            }
        }

        public async void Info(string message)
        {
            Log.Logger.Information(message);

            try
            {
                await _telegramService.MakeNotification($"💙 {message}");
            }
            catch (Exception exсeption)
            {
                Log.Logger.Error(exсeption, TelegramNotificationError);
            }
        }
    }
}
