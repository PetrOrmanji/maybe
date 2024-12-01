using Quartz;
using EnsureThat;
using Maybe.App.Serivces;

namespace Maybe.App.Jobs
{
    internal class SaveChangesJob : IJob
    {
        private readonly IdeasService _ideasService;
        private readonly LoggerService _loggerService;

        public SaveChangesJob(IdeasService ideasService, LoggerService loggerService)
        {
            EnsureArg.IsNotNull(ideasService, nameof(ideasService));

            _ideasService = ideasService;
            _loggerService = loggerService;
        }

        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                _ideasService.SaveChanges();
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Ошибка при удалении запланированных задач");
            }

            return Task.CompletedTask;
        }
    }
}
