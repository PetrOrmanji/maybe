using EnsureThat;
using Maybe.App.Serivces;
using Quartz;

namespace Maybe.App.Jobs
{
    public class PublicateJob : IJob
    {
        private readonly TelegramService _telegramService;
        private readonly IdeasService _ideasService;
        private readonly LoggerService _loggerService;

        public PublicateJob(TelegramService telegramService, IdeasService ideasService, LoggerService loggerService)
        {
            EnsureArg.IsNotNull(telegramService, nameof(telegramService));
            EnsureArg.IsNotNull(ideasService, nameof(ideasService));
            EnsureArg.IsNotNull(loggerService, nameof(loggerService));

            _telegramService = telegramService;
            _ideasService = ideasService;
            _loggerService = loggerService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var notPublicatedCount = _ideasService.Count(false);

            if (notPublicatedCount == 0)
            {
                _loggerService.Warn("Идеи для публикации закончились");
                return;
            }
            else if (notPublicatedCount < 50)
            {
                _loggerService.Warn($"Неопубликованных идей осталось: {notPublicatedCount}");
            }

            var idea = _ideasService.GetNotPublicatedIdea();
            if(idea == null)
            {
                return;
            }

            try
            {
                await _telegramService.MakePublication(idea);
                idea.Publicated = DateTime.Now;
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Ошибка при создании публикации");
            }
        }
    }
}
