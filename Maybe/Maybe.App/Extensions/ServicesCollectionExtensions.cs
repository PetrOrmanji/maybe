using EnsureThat;
using Maybe.App.Jobs;
using Maybe.App.Serivces;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace Maybe.App.Extensions
{
    public static class ServicesCollectionExtensions
    {
        public static void AddTelegram(this IServiceCollection services, string botToken, string channelId, string notifyChannelId)
        {
            EnsureArg.IsNotNull(services, nameof(services));

            services.AddSingleton(provider => new TelegramService(botToken, channelId, notifyChannelId));
        }

        public static void AddLogger(this IServiceCollection services)
        {
            EnsureArg.IsNotNull(services, nameof(services));
            
            services.AddSingleton(provider => new LoggerService(provider.GetRequiredService<TelegramService>()));
        }

        public static void AddIdeas(this IServiceCollection services, string path)
        {
            EnsureArg.IsNotNull(services, nameof(services));
            EnsureArg.IsNotNullOrWhiteSpace(path, nameof(path));
            if(!File.Exists(path)) throw new Exception("Указанного файла источника не существует");

            services.AddSingleton(services => new IdeasService(path));
        }

        public static void AddJobs(this IServiceCollection services)
        {
            EnsureArg.IsNotNull(services, nameof(services));

            services.AddQuartz(x =>
            {
                var createTasksJobKey = new JobKey("CreateTasksJob");
                x.AddJob<CreateJob>(opts => opts
                .WithIdentity(createTasksJobKey)
                .StoreDurably());

                x.AddTrigger(opts => opts
                .ForJob(createTasksJobKey)
                .WithIdentity("CreateTasksTrigger")
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(0, 10)));

                var saveChangesJobKey = new JobKey("SaveChangesJob");
                x.AddJob<SaveChangesJob>(opts => opts
                .WithIdentity(saveChangesJobKey)
                .StoreDurably());

                x.AddTrigger(opts => opts
                .ForJob(saveChangesJobKey)
                .WithIdentity("SaveChangesTrigger")
                .WithSimpleSchedule(opts => opts.WithIntervalInHours(1)));
            });

            services.AddQuartzHostedService(x => x.WaitForJobsToComplete = true);
        }
    }
}
