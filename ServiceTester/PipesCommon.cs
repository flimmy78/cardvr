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
		public enum Ident
		{
			BasicInformation = 1,
		}

		public interface IPacket
		{
			int size();
			byte[] toBytes();
		}

		public class Request : IPacket
		{
			public Request(Ident id)
			{
				id_ = id;
			}

			public int size()
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
			public BasicInformation(byte[] raw)
			{
				int index = 0;
				isRecording_ = raw[index++] == 1 ? true : false;
				speed_ = (int)raw[index++];
				fps_ = (int)raw[index++];
			}

			bool isRecording_;
			int speed_;
			int fps_;

			public int size()
			{
				return 4;
			}

			public byte[] toBytes()
			{
				byte[] result = new byte[size()];

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
