using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Pipes;
using System.Threading;

namespace CarDvrPipes
{
    public class PipesServer
    {
        const int connectionTimeOut = 10000;
        const int maxPipeInstances = 2;
        object connectionGuard = new object();
        bool connected = false;
        NamedPipeServerStream pipeStream = new NamedPipeServerStream(PipesCommon.CarDvrPipeName, PipeDirection.InOut, maxPipeInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
        
        Thread connectionThread = null;

        private void WaitConnectionCallback(IAsyncResult result)
        {
            try
            {
                pipeStream.EndWaitForConnection(result);
                lock (connectionGuard)
                {
                    connected = true;
                }
            }
            catch (OperationCanceledException)
            {
                // LOG            	
            }

            System.IO.File.Create(connected ? "c:/connected.test" : "c:/timedout.test").Close();
        }

        void ConnectToPipe()
        {
            pipeStream.BeginWaitForConnection(WaitConnectionCallback, null);
        }

        /// <summary>
        /// This thread should always check availability of the pipe and should check that CarDVR works fine
        /// </summary>
        void ConnectionThread()
        {
            Thread connectThread = new Thread(ConnectToPipe);
            connectThread.Start();

            int timeout = 0;
            while (timeout < connectionTimeOut)
            {
                lock (connectionGuard)
                {
                    if (connected)
                        break;
                }

                Thread.Sleep(100);
                timeout += 100;
            }
        }

        public void Start()
        {
            connectionThread = new Thread(ConnectionThread);
        }

        public void Stop()
        {
            if (connectionThread == null)
                return;

            connectionThread.Interrupt();
            connectionThread.Join();

            connectionThread = null;
        }

        public bool IsConnected()
        {
            lock (connectionGuard)
            {
                return connected;
            }
        }
    }
}
