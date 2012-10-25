﻿using System.ComponentModel;
using System.Diagnostics;

namespace LogMonitor
{
    public partial class WindowsService
    {
        /// <summary> 
        /// Erforderliche Designervariable.
        /// </summary>
        private IContainer components = null;
        
        private EventLog applicationEventLog;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.components != null)
                {
                    this.components.Dispose();
                }

                if (this.kernel != null)
                {
                    this.kernel.Dispose();
                    this.kernel = null;
                }
            }

            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.applicationEventLog = new EventLog();
            ((ISupportInitialize)this.applicationEventLog).BeginInit();

            // 
            // ApplicationEventLog
            // 

            this.applicationEventLog.Log = "Application";
            this.applicationEventLog.Source = "LogMonitor";

            // 
            // WindowsService
            // 

            this.ServiceName = "LogMonitor";
            ((ISupportInitialize)this.applicationEventLog).EndInit();
        }
    }
}
