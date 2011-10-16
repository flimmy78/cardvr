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
        static void Main(string[] args)
        {
            PipesServer test = new PipesServer();
            test.Start();

			test.gotPacketEvent += PacketEventHandler_;
        }

		static void PacketEventHandler_(object sender, CarDvrPipes.PipesServer.PacketEventArgs e)
		{
			CarDvrPipes.Packets.BasicInformation bi = e.packet as CarDvrPipes.Packets.BasicInformation;
			
			if (bi == null)
				return;

			System.Media.SoundPlayer player = new System.Media.SoundPlayer();
			player.SoundLocation = @"C:\Dev\cardvr\CarDVR\voice\test.wav";
			player.Play();
		}
    }
}
