using System;

namespace FeatureManagementWorkerDemo
{
    public class WorkerOptions
    {
        public string Message { get; set; }

        public TimeSpan Interval { get; set; } = TimeSpan.FromSeconds(5);
    }
}
