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
        const int readingTimeOut = 10000;
        const int maxPipeInstances = 2;
        
        object connectionGuard = new object();
        bool connected = false;

        NamedPipeServerStream pipeStream = new NamedPipeServerStream(PipesCommon.CarDvrPipeName, PipeDirection.InOut, maxPipeInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
        
        Thread connectionThread = null;
        Thread workerThread = null;

        ///// <summary>
        ///// Callback, which says that pipe connection finished
        ///// </summary>
        //private void WaitConnectionCallback(IAsyncResult result)
        //{
        //    try
        //    {
        //        pipeStream.EndWaitForConnection(result);
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        // LOG            	
        //    }
        //}

        /// <summary>
        /// This thread should try to connect to CarDVR all time
        /// </summary>
        void ConnectionThread()
        {
            while (true)
            {
                try
                {
                    if (!pipeStream.IsConnected)
                        pipeStream.WaitForConnection();

                }
                catch (Exception e)
                {
                    pipeStream.WaitForConnection();
                    /*pipeStream.BeginWaitForConnection(WaitConnectionCallback, null);*/
                }

                Thread.Sleep(5000);
            }
        }

        void PipeWorker()
        {
            int timeout = 0;
            int position = 0;

            while (true)
            {
                try
                {
                    while (!pipeStream.IsConnected)
                        Thread.Sleep(100);

                    byte[] checkOkPacket = new byte[] { 1, 2, 3, 4, 5 };

                    pipeStream.Write(checkOkPacket, 0, checkOkPacket.Length);

                    if (pipeStream.CanRead)
                    {
                        byte[] result = new byte[100];
                        int realCoont = pipeStream.Read(result, position, result.Length);
                        position += realCoont;

                        if (realCoont >= 5)
                        {
                            if (result[0] == 1 && result[1] == 2 && result[2] == 3 && result[3] == 4 && result[4] == 5)
                            {
                                // everything is fine!
                                timeout = 0;

                                result[0] = 0;

                                // wait for 10 secs
                                Thread.Sleep(10000);
                            }
                        }
                    }
                    if (timeout >= readingTimeOut)
                    {
                        // LOG!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        // try to connect again

                        timeout = 0;
                        pipeStream.Disconnect();
                    }

                    Thread.Sleep(100);
                    timeout += 100;

                }
                catch (Exception e)
                {
                }
            }
        }

        public void Start()
        {
            connectionThread = new Thread(ConnectionThread);
            connectionThread.Start();

            workerThread = new Thread(PipeWorker);
            workerThread.Start();
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
