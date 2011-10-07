using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO.Pipes;
using System.Threading;

using CarDvrPipes;

namespace CarDvrService
{
    public partial class CarDvrService : ServiceBase
    {
        public CarDvrService()
        {
            InitializeComponent();
        }

        PipesServer pipes = null;

        /// <summary>
        /// Initialize worker thread and finish as soon as possible
        /// </summary>
        protected override void OnStart(string[] args)
        {
            pipes = new PipesServer();
            pipes.Start();            
        }

        /// <summary>
        /// Stops worker thread and close pipe
        /// </summary>
       protected override void OnStop()
       {
           pipes.Stop();
        }
    }
}
