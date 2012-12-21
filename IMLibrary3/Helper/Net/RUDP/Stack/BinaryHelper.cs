using System;
using System.IO;
using System.Net;
using System.Text;

namespace Helper.Net.RUDP
{

	/// <summary>
	/// Helper to read/write in a binary array
	/// </summary>
	internal static class BinaryHelper
	{

		#region Read ...

		internal static bool ReadBool(byte[] bin, int offset)
		{
			return (bin[offset] > 0);
		}

		internal static int ReadInt(byte[] bin, int offset)
		{
			int val = 0;
			for (int i = 0; i < 4; i++)
			{
				val = (val << 8) | bin[i + offset];
			}
			return val;
		}

		internal static long ReadLong(byte[] bin, int offset)
		{
			long val = 0;
			for (int i = 0; i < 8; i++)
				val = (val << 8) | bin[i + offset];

			return val;
		}

		internal static short ReadShort(byte[] bin, int offset)
		{
			return (short)((bin[offset] << 8) | bin[offset + 1]);
		}

		internal static string ReadString(byte[] bin, int offset, out int bytelength)
		{
			// Find the end of the string:
			int string_end = offset;
			while (bin[string_end] != 0)
				string_end++;

			// Add 1 for the null terminator
			bytelength = string_end - offset + 1;
			Encoding e = Encoding.UTF8;

			// Subtract 1 for the null terminator
			return e.GetString(bin, offset, bytelength - 1);
		}

		internal static float ReadFloat(byte[] bin, int offset)
		{
			if (BitConverter.IsLittleEndian)
			{
				//Console.WriteLine("This machine uses Little Endian processor!");
				SwapEndianism(bin, offset, 4);
				float result = BitConverter.ToSingle(bin, offset);
				//Swap it back:
				SwapEndianism(bin, offset, 4);
				return result;
			}
			else
				return BitConverter.ToSingle(bin, offset);
		}

		internal static bool ReadFlag(byte[] bin, int offset)
		{
			byte var = (byte)(0x80 & bin[offset]);
			if (var == 0x80)
				return true;

			return false;
		}

		#endregion

		#region Write

		internal static void WriteBool(bool val, byte[] target, int offset)
		{
			if (val)
				target[offset] = 1;
			else
				target[offset] = 0;
		}

		internal static void WriteInt(int val, byte[] target, int offset)
		{
			for (int i = 0; i < 4; i++)
				target[offset + i] = (byte)(0xFF & (val >> 8 * (3 - i)));
		}

		internal static void WriteUInt(uint val, byte[] target, int offset)
		{
			for (int i = 0; i < 4; i++)
				target[offset + i] = (byte)(0xFF & (val >> 8 * (3 - i)));
		}

		internal static void WriteShort(short val, byte[] target, int offset)
		{
			target[offset] = (byte)(0xFF & (val >> 8));
			target[offset + 1] = (byte)(0xFF & (val));
		}

		internal static void WriteUShort(ushort val, byte[] target, int offset)
		{
			target[offset] = (byte)(0xFF & (val >> 8));
			target[offset + 1] = (byte)(0xFF & (val));
		}

		internal static int WriteString(string svalue, byte[] target, int offset)
		{
			Encoding e = Encoding.UTF8;
			int bcount = e.GetBytes(svalue, 0, svalue.Length, target, offset);

			// Write the null
			target[offset + bcount] = 0;
			return bcount + 1;
		}

		internal static void WriteLong(long lval, byte[] target, int offset)
		{
			for (int i = 0; i < 8; i++)
			{
				byte tmp = (byte)(0xFF & (lval >> 8 * (7 - i)));
				target[i + offset] = tmp;
			}
		}

		internal static void WriteFloat(float value, byte[] target, int offset)
		{
			byte[] arr = BitConverter.GetBytes(value);
			if (BitConverter.IsLittleEndian)
			{
				// Make sure we are Network Endianism
				SwapEndianism(arr, 0, 4);
			}

			Array.Copy(arr, 0, target, offset, 4);
		}

		internal static void WriteFlag(bool flag, byte[] target, int offset)
		{
			byte var = target[offset];
			if (flag)
				var |= 0x80;    //Make the first bit 1
			else
				var &= 0x7F;    //Make the first bit 0

			target[offset] = var;
		}

		#endregion

		#region GetByteCount

		internal static int GetByteCount(string s)
		{
			// We just need one more byte than the UTF8 encoding does
			return Encoding.UTF8.GetByteCount(s) + 1;
		}

		#endregion

		#region Helpers

		private static void SwapEndianism(byte[] data, int offset, int length)
		{
			int steps = length / 2;
			for (int i = 0; i < steps; i++)
			{
				byte tmp = data[offset + i];
				data[offset + i] = data[offset + length - i - 1];
				data[offset + length - i - 1] = tmp;
			}
		}

		#endregion

	}

}
