﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LogMonitor.Processors;

namespace LogMonitor
{
    internal class ChangeManager : IDisposable
    {
        private readonly BlockingCollection<FileChange> inputQueue = new BlockingCollection<FileChange>();

        private readonly BlockingCollection<ResultContainer> outputQueue = new BlockingCollection<ResultContainer>();

        private readonly IProcessor[] processors;

        private readonly CancellationTokenSource cancellation;
        
        private readonly TaskFactory factory;

        private bool disposed;

        public ChangeManager(IEnumerable<IProcessor> processors)
        {
            if (processors == null)
                throw new ArgumentNullException("processors");

            this.cancellation = new CancellationTokenSource();

            this.processors = processors.ToArray();

            this.factory = new TaskFactory(this.cancellation.Token);
            
            this.factory.StartNew(() => this.ProcessInput(), TaskCreationOptions.LongRunning);
            this.factory.StartNew(() => this.ProcessMetrics(), TaskCreationOptions.LongRunning);
        }

        public void Add(FileChange item)
        {
            this.inputQueue.Add(item);
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this.disposed)
            {
                this.cancellation.Cancel();

                this.disposed = true;
            }
        }

        private void ProcessInput()
        {
            while (!this.cancellation.IsCancellationRequested)
            {
                FileChange item = this.inputQueue.Take(this.cancellation.Token);

                if (this.cancellation.IsCancellationRequested)
                    return;

                IProcessor[] matchingProcessors = this.processors
                    .Where(p => p.IsMatch(item.File))
                    .ToArray();

                IEnumerable<Metric>[] metrics = new IEnumerable<Metric>[matchingProcessors.Length];

                Sequential.For(
                    0,
                    matchingProcessors.Length,
                    i => metrics[i] = matchingProcessors[i].ParseLine(item));

                this.outputQueue.Add(new ResultContainer
                {
                    Change = item,
                    Metrics = metrics.SelectMany(m => m).ToArray()
                });
            }
        }

        private void ProcessMetrics()
        {
            while (!this.cancellation.IsCancellationRequested)
            {
                ResultContainer item = this.outputQueue.Take(this.cancellation.Token);

                if (this.cancellation.IsCancellationRequested)
                    return;
            }
        }
    }
}
