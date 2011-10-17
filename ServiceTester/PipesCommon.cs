using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarDvrPipes
{
    class PipesCommon
    {
        public static string CarDvrPipeName = "CarDvrPipe";
    }

	namespace Packets
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

		public class Helper
		{
			public const int MaximalPossiblePacketLength = 1024;
            public static int[] PacketsSizes = new int[] { 0, BasicInformation.Size };

            public static bool isValidIdent(byte[] raw, int readBytes)
            {
                if (raw.Length == 0)
                    return false;
                
                byte id = raw[0];
                return (id > 0) && (id < (byte)Ident.Max) && (PacketsSizes[id] == readBytes);
            }
		}

		public class Instance
		{
			public static IPacket Make(Ident id, byte[] data)
			{
				switch (id)
				{
					case Ident.BasicInformation:
                        return new BasicInformation(data);
				}

				return null;
			}
		}

		public enum Ident
		{
			BasicInformation = 1,
			Max
		}

		public interface IPacket
		{
            int DynamicSizeOf();
			byte[] toBytes();
		}

		public class Request : IPacket
		{
			public Request(Ident id)
			{
				id_ = id;
			}

            public int DynamicSizeOf()
            {
                return 1;
            }

			public byte[] toBytes()
			{
				return new byte[] { (byte)id_ };
			}

			Ident id_;
		}

		public class BasicInformation : IPacket
		{
			public BasicInformation(bool isRecording, int speed, int fps)
			{
				isRecording_ = isRecording;
				speed_ = speed;
				fps_ = fps;
			}

            public BasicInformation(byte[] data)
			{
				int index = 0;
                isRecording_ = data[index++] == 1 ? true : false;
                speed_ = (int)data[index++];
                fps_ = (int)data[index++];
			}

			bool isRecording_;
			int speed_;
			int fps_;

			public static int Size = 4;

            public int DynamicSizeOf()
            {
                return Size;
            }

			public byte[] toBytes()
			{
				byte[] result = new byte[Size];

				int index = 0;
				result[index++] = (byte)Ident.BasicInformation;
				result[index++] = (byte)(isRecording_ ? 1 : 0);
				result[index++] = (byte)speed_;
				result[index++] = (byte)fps_;

				return result;
			}
		}
	}
}
