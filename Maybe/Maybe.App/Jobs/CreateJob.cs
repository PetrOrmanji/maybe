using EnsureThat;
using Maybe.App.Serivces;
using Quartz;

namespace Maybe.App.Jobs
{
    public class CreateJob : IJob
    {
        private readonly LoggerService _loggerService;

        public CreateJob(LoggerService loggerService)
        {
            EnsureArg.IsNotNull(loggerService, nameof(loggerService));

            _loggerService = loggerService;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var scheduler = context.Scheduler;

            try
            {
                var jobs = GetJobs();
                var triggers = GetTriggers();

                await ScheduleJob(scheduler, jobs[0], triggers[0]);
                await ScheduleJob(scheduler, jobs[1], triggers[1]);
                await ScheduleJob(scheduler, jobs[2], triggers[2]);
                await ScheduleJob(scheduler, jobs[3], triggers[3]);
                await ScheduleJob(scheduler, jobs[4], triggers[4]);

                _loggerService.Info($"План публикаций \r\n" +
                    $"1. {triggers[0].StartTimeUtc.DateTime}, \r\n" +
                    $"2. {triggers[1].StartTimeUtc.DateTime}, \r\n" +
                    $"3. {triggers[2].StartTimeUtc.DateTime}, \r\n" +
                    $"4. {triggers[3].StartTimeUtc.DateTime}, \r\n" +
                    $"5. {triggers[4].StartTimeUtc.DateTime}");
            }
            catch (Exception ex)
            {
                _loggerService.Error(ex, "Ошибка при создании запланированных задач");
            }
        }

        private async Task ScheduleJob(IScheduler scheduler, IJobDetail job, ITrigger trigger)
        {
            if (trigger.StartTimeUtc < DateTimeOffset.UtcNow)
            {
                return;
            }

            await scheduler.ScheduleJob(job, trigger);
        }

        private IJobDetail[] GetJobs()
        {
            return
            [
                JobBuilder.Create<PublicateJob>().WithIdentity("PublicateTask1", "PublicateTaskGroup1").Build(),
                JobBuilder.Create<PublicateJob>().WithIdentity("PublicateTask2", "PublicateTaskGroup2").Build(),
                JobBuilder.Create<PublicateJob>().WithIdentity("PublicateTask3", "PublicateTaskGroup3").Build(),
                JobBuilder.Create<PublicateJob>().WithIdentity("PublicateTask4", "PublicateTaskGroup4").Build(),
                JobBuilder.Create<PublicateJob>().WithIdentity("PublicateTask5", "PublicateTaskGroup5").Build(),
            ];
        }

        private ITrigger[] GetTriggers()
        {
            var randomDateTimes = GetRandomDateTimes();

            return
            [
                TriggerBuilder.Create().WithIdentity("Trigger1", "PublicateTaskGroup1").StartAt(randomDateTimes[0]).Build(),
                TriggerBuilder.Create().WithIdentity("Trigger2", "PublicateTaskGroup2").StartAt(randomDateTimes[1]).Build(),
                TriggerBuilder.Create().WithIdentity("Trigger3", "PublicateTaskGroup3").StartAt(randomDateTimes[2]).Build(),
                TriggerBuilder.Create().WithIdentity("Trigger4", "PublicateTaskGroup4").StartAt(randomDateTimes[3]).Build(),
                TriggerBuilder.Create().WithIdentity("Trigger5", "PublicateTaskGroup5").StartAt(randomDateTimes[4]).Build(),
            ];
        }

        private DateTime[] GetRandomDateTimes()
        {
            var random = new Random();

            return
            [
                DateTime.Now.Date.AddMinutes(TimeSpan.FromHours(7).Add(TimeSpan.FromMinutes(random.Next(0, 180))).TotalMinutes),
                DateTime.Now.Date.AddMinutes(TimeSpan.FromHours(10).Add(TimeSpan.FromMinutes(random.Next(0, 180))).TotalMinutes),
                DateTime.Now.Date.AddMinutes(TimeSpan.FromHours(13).Add(TimeSpan.FromMinutes(random.Next(0, 240))).TotalMinutes),
                DateTime.Now.Date.AddMinutes(TimeSpan.FromHours(17).Add(TimeSpan.FromMinutes(random.Next(0, 60))).TotalMinutes),
                DateTime.Now.Date.AddMinutes(TimeSpan.FromHours(18).Add(TimeSpan.FromMinutes(random.Next(0, 120))).TotalMinutes)
            ];
        }
    }
}
