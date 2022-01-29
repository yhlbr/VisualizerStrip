using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VisualizerStrip.Visualization;

namespace VisualizerStrip
{
    public class Worker
    {
        private static Worker instance = null;
        private static readonly object padlock = new object();

        private SpectrumService service = null;
        private BackgroundWorker worker = null;

        private Worker()
        {
            service = new SpectrumService();
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = false;
            worker.WorkerSupportsCancellation = true;
        }

        public void start()
        {
            if (worker.IsBusy != true)
            {
                worker.RunWorkerAsync();
            }
        }

        public void stop()
        {
            if (worker.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                worker.CancelAsync();
            }
        }

        // This event handler is where the time-consuming work is done.
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            for (int i = 1; i <= 10; i++)
            {
                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    // Perform a time consuming operation and report progress.
                    System.Threading.Thread.Sleep(500);
                    worker.ReportProgress(i * 10);
                }
            }
        }

        public static Worker Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Worker();
                    }
                    return instance;
                }
            }
        }

    }
}
