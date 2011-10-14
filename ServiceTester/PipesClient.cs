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
        void PipeReadingThread()
        {
            //while (true)
            {
                if (pipeStream.CanRead)
                {
                    byte[] result = new byte[100];

                    int realCount = pipeStream.Read(result, 0, result.Length);

                    result[0] = 1;
                    result[1] = 2;
                    result[2] = 3;
                    result[3] = 4;
                    result[4] = 5;

                    pipeStream.Write(result, 0, 5);
                    
                }

                Thread.Sleep(10000);
            }

        }

        Thread clientPipeThread = null;

        public void Start()
        {
            try
            {
                pipeStream.Connect(1000);
                clientPipeThread = new Thread(PipeReadingThread);
                clientPipeThread.Start();                 
            }
            catch (Exception )
            {
                // LOG
            	
            }
        }

        public void Stop()
        {
            clientPipeThread.Interrupt();
            clientPipeThread.Join();

            pipeStream.Close();
        }

        public bool IsConnected()
        {
            return pipeStream.IsConnected;
        }
    }
}
