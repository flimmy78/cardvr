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
		public class PacketEventArgs : EventArgs
		{
			public PacketEventArgs(Packets.IPacket pkt)
			{
				packet = pkt;
			}
			public Packets.IPacket packet;
		}
		public delegate void PacketEventHandler(object sender, PacketEventArgs e);
		public PacketEventHandler gotPacketEvent;

        const int readingTimeOut = 10000;
        const int maxPipeInstances = 2;
        
        object stateGuard = new object();
        //bool connected = false;
		//bool broken = false;

        NamedPipeServerStream pipeStream = null; //new NamedPipeServerStream(PipesCommon.CarDvrPipeName, PipeDirection.InOut, maxPipeInstances, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
        
        Thread connectionThread = null;
        Thread workerThread = null;
		Queue<byte[]> packetsToSend = new Queue<byte[]>();


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
			byte[] informationRequest = new Packets.Request(Packets.Ident.BasicInformation).toBytes();
			Packets.BasicInformation bi = new Packets.BasicInformation(false, 0, 0);

            while (true)
            {
                try
                {
                    while (!pipeStream.IsConnected)
                        Thread.Sleep(100);

					pipeStream.Write(informationRequest, 0, informationRequest.Length);

                    if (pipeStream.CanRead)
                    {
						byte[] result = new byte[bi.size()];

                        if (bi.size() == pipeStream.Read(result, 0, result.Length))
                        {
							if (result[0] == (byte)Packets.Ident.BasicInformation)
							{
								if (gotPacketEvent != null)
									gotPacketEvent(this, new PacketEventArgs(new Packets.BasicInformation(result)));
							}

							//if (result[0] == (byte)Packets.Ident.BasicInformation)
							//{
							//    System.Media.SoundPlayer player = new System.Media.SoundPlayer();

							//    player.SoundLocation = @"C:\Dev\cardvr\CarDVR\voice\test.wav";
							//    player.Play();

							//    // wait for 10 secs
							//    Thread.Sleep(10000);
							//}

							Thread.Sleep(10000);
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

		//public bool IsConnected()
		//{
		//    lock (connectionGuard)
		//    {
		//        return connected;
		//    }
		//}
    }
}
