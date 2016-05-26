using Atlas;
using Common.Logging;
using Quartz;
using Quartz.Spi;
using System.Configuration;
using System;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace Honshu.WindowsService
{
    /// <summary>
    /// This represents an entity for Windows Service hosted by Atlas.
    /// </summary>
    internal class SampleService : IAmAHostedProcess
    {
        private static readonly ILog Log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets or sets the scheduler instance.
        /// </summary>
        public IScheduler Scheduler { get; set; }           // #1

        /// <summary>
        /// Gets or sets the job factory instance.
        /// </summary>
        public IJobFactory JobFactory { get; set; }         // #2

        /// <summary>
        /// Gets or sets the job listener instance.
        /// </summary>
        public IJobListener JobListener { get; set; }       // #3

        /// <summary>
        /// Starts the Windows Service.
        /// </summary>
        public void Start()
        {
            //Log.Info("Sample Windows Service starting");

            if (ConfigurationManager.AppSettings.Get("TaobaoProductFetcherJob").ToLower() == "open")
            {
                AddJobForSchelduler<TaobaoProductFetcherJob>("TaobaoProductFetcherJob", "CronExpressionEvery3Min");
            }
            if (ConfigurationManager.AppSettings.Get("ElasticSearchProductAsyncJob").ToLower() == "open")
            {
                AddJobForSchelduler<ElasticSearchProductAsyncJob>("ElasticSearchProductAsyncJob",
                    "ElasticSearchProduct");
            }

            if (ConfigurationManager.AppSettings.Get("ElasticSearchShopAsyncJob").ToLower() == "open")
            {
                AddJobForSchelduler<ElasticSearchShopAsyncJob>("ElasticSearchShopAsyncJob",
                    "ElasticSearchShop");
            }

            this.Scheduler.ListenerManager.AddJobListener(this.JobListener);    // #9
            this.Scheduler.Start();                         // #10

            //Log.Info("Sample Windows Service started");
        }

        private void AddJobForSchelduler<T>(string jobName,string cronExpression) where T :IJob
        {
            var job = JobBuilder.Create<T>()
                                .WithIdentity(jobName, "Honshu.WindowsService")
                                .Build();                   // #4

            var trigger = TriggerBuilder.Create()
                                        .WithIdentity(jobName + "SampleTrigger", "Honshu.WindowsService")
                                        .WithCronSchedule(ConfigurationManager.AppSettings[cronExpression])   // #5
                                        .ForJob(jobName, "Honshu.WindowsService")
                                        .Build();           // #6

            this.Scheduler.JobFactory = this.JobFactory;    // #7
            this.Scheduler.ScheduleJob(job, trigger);       // #8
        }


        #region job helper

        /// <summary>
        /// Stops the Windows Service.
        /// </summary>
        public void Stop()
        {
            Log.Info("Sample Windows Service stopping");

            this.Scheduler.Shutdown();

            Log.Info("Sample Windows Service stopped");
        }

        /// <summary>
        /// Resumes the Windows Service.
        /// </summary>
        public void Resume()
        {
            Log.Info("Sample Windows Service resuming");

            this.Scheduler.ResumeAll();

            Log.Info("Sample Windows Service resumed");
        }

        /// <summary>
        /// Pauses the Windows Service.
        /// </summary>
        public void Pause()
        {
            Log.Info("Sample Windows Service pausing");

            this.Scheduler.PauseAll();

            Log.Info("Sample Windows Service paused");
        }
        #endregion
    }
}