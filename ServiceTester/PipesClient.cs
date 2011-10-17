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
        NamedPipeClientStream pipeStream = null;

		void CreateClientPipe()
		{
			pipeStream = new NamedPipeClientStream(".", PipesCommon.CarDvrPipeName, PipeDirection.InOut);
			pipeStream.Connect(10000);
		}

        /// <summary>
        /// This thread should always check availability of the pipe and should check that CarDVR works fine
        /// </summary>
        void PipeReadingThread()
        {
			byte[] result = new byte[Packets.BasicInformation.Size];

            while (true)
            {
				try
				{
					if (pipeStream.CanRead)
					{
						int realCount = pipeStream.Read(result, 0, result.Length);

						if (result[0] == (byte)Packets.Ident.BasicInformation)
						{
							Packets.BasicInformation basicInformation = new Packets.BasicInformation
							(
								true,
								120,
								30
							);

							byte[] data = basicInformation.toBytes();
							pipeStream.Write(data, 0, data.Length);
						}
					}
					Thread.Sleep(1000);
				}
				catch (Exception e)
				{
					try
					{
						pipeStream.Close();
						CreateClientPipe();
					}
					catch (Exception ee)
					{
						// ERROR
					}
				}                
            }

        }

        Thread clientPipeThread = null;

        public void Start()
        {
            try
            {
				CreateClientPipe();
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
            clientPipeThread.Abort();
            clientPipeThread.Join();

            pipeStream.Close();
        }

        public bool IsConnected()
        {
            return pipeStream.IsConnected;
        }
    }
}
