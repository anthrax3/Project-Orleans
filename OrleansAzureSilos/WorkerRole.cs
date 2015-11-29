using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Orleans.Runtime.Host;

namespace OrleansAzureSilos
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        Orleans.Runtime.Host.AzureSilo silo;

        public override void Run()
        {
            var config = new ClusterConfiguration();
            config.StandardLoad();

            // Configure storage providers

            silo = new AzureSilo();
            bool ok = silo.Start(RoleEnvironment.DeploymentId,
                                 RoleEnvironment.CurrentRoleInstance,
                                 config);

            silo.Run(); // Call will block until silo is shutdown

            Trace.TraceInformation("OrleansAzureSilos is running");

            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("OrleansAzureSilos has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("OrleansAzureSilos is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            silo.Stop();
            base.OnStop();

            Trace.TraceInformation("OrleansAzureSilos has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}
