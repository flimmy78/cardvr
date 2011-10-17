using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.Pipes;
using CarDvrPipes;

namespace ServiceTester
{
    class Program
    {
        static PipesServer carDvrPipe = null;

        static Thread cardvrAskingThread = null;

        static void Main(string[] args)
        {
            carDvrPipe = new PipesServer();
            carDvrPipe.Start();

            cardvrAskingThread = new Thread(AskingThread);
            cardvrAskingThread.Start();

            Subscribe();
            
            carDvrPipe.WaitForFinish();

            Unsubscribe();

            cardvrAskingThread.Abort();
            cardvrAskingThread.Join();
        }

        static void AskingThread()
        {
            while (true)
            {
                carDvrPipe.AddPacketToQueue(new CarDvrPipes.Packets.Request(CarDvrPipes.Packets.Ident.BasicInformation));
                Thread.Sleep(10000);
            }
        }

        static void Subscribe()
        {
            if (carDvrPipe != null)
                carDvrPipe.gotPacketEvent += PacketEventHandler_;
        }

        static void Unsubscribe()
        {
            if (carDvrPipe != null)
                carDvrPipe.gotPacketEvent -= PacketEventHandler_;
        }

		static void PacketEventHandler_(object sender, CarDvrPipes.Packets.PacketEventArgs e)
		{
			CarDvrPipes.Packets.BasicInformation bi = e.packet as CarDvrPipes.Packets.BasicInformation;
			
			if (bi == null)
				return;

			System.Media.SoundPlayer player = new System.Media.SoundPlayer();
			player.SoundLocation = @"voice\test.wav";
			player.Play();
		}
    }
}
