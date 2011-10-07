using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Pipes;
using System.Threading;

namespace CarDvrPipes
{
    public class PipesClient
    {
        const int connectionTimeOut = 10000;
        const int maxPipeInstances = 2;
        object connectionGuard = new object();
        bool connected = false;
        NamedPipeClientStream pipeStream = new NamedPipeClientStream(".", PipesCommon.CarDvrPipeName, PipeDirection.InOut);
        //, maxPipeInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

  
        /// <summary>
        /// This thread should always check availability of the pipe and should check that CarDVR works fine
        /// </summary>

        public void Start()
        {
            try
            {
                pipeStream.Connect(1000);
            }
            catch (Exception )
            {
                // LOG
            	
            }
        }

        public void Stop()
        {
            pipeStream.Close();
        }

        public bool IsConnected()
        {
            return pipeStream.IsConnected;
        }
    }
}
