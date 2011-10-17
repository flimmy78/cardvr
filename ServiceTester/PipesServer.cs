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
		public Packets.PacketEventHandler gotPacketEvent;

        const int maxPipeInstances = 2;
        const int readingTimeOut = 10000;        // ms
        const int clientAckTimeInterval = 10000; // ms
        const int clientConnectedTimeInterval = 1000; // ms
        
        NamedPipeServerStream pipeStream = null;
        
        Thread connectionThread = null;
        Thread workerThread = null;

        object queueGuard = new object();
        Queue<Packets.IPacket> packetsToSend = new Queue<Packets.IPacket>();


        public void AddPacketToQueue(Packets.IPacket packet)
        {
            lock (queueGuard)
            {
                packetsToSend.Enqueue(packet);
            }
            Monitor.Wait(packetsToSend);
        }

		void CreatePipe()
		{
			pipeStream = new NamedPipeServerStream(PipesCommon.CarDvrPipeName, PipeDirection.InOut, maxPipeInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
		}

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
					pipeStream.Close();
					CreatePipe();
                }

                Thread.Sleep(5000);
            }
        }

        void PipeWorker()
        {
            int timeout = 0;
			//byte[] informationRequest = new Packets.Request(Packets.Ident.BasicInformation).toBytes();
			byte[] result = new byte[Packets.Helper.MaximalPossiblePacketLength];

            while (true)
            {
                try
                {
                    Monitor.Wait(packetsToSend);

                    while (!pipeStream.IsConnected)
                        Thread.Sleep(clientConnectedTimeInterval);

                    Packets.IPacket packet = packetsToSend.Dequeue();
                    byte[] rawpacket = packet.toBytes();
                    pipeStream.Write(rawpacket, 0, rawpacket.Length);

                    if (pipeStream.CanRead)
                    {
						int bytesAmount = pipeStream.Read(result, 0, result.Length);
                        if (Packets.Helper.isValidIdent(result, bytesAmount))
                        {
                            if (gotPacketEvent != null)
                                gotPacketEvent(this, new Packets.PacketEventArgs(new Packets.BasicInformation(result)));

                            break;
                        }

                        //Thread.Sleep(clientAckTimeInterval);
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
			CreatePipe();

            connectionThread = new Thread(ConnectionThread);
            connectionThread.Start();

            workerThread = new Thread(PipeWorker);
            workerThread.Start();
        }

        public void Stop()
        {
            if (connectionThread == null)
                return;

            connectionThread.Abort();
            connectionThread.Join();

            connectionThread = null;
        }

        public void WaitForFinish()
        {
            if (connectionThread != null)
                connectionThread.Join();
        }
    }
}
