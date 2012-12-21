using System;
using System.IO;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IMLibrary.AV
{

	/// <summary>
	/// G729 的摘要说明。
	/// </summary>
	public class G729
	{
		const int L_FRAME_COMPRESSED=10;
		const int L_FRAME=80;
		
		[DllImport("g729",PreserveSig=true)]
		private extern static void va_g729a_init_encoder();
		[DllImport("g729")]
		private extern static void va_g729a_encoder([MarshalAs(UnmanagedType.LPArray)]byte[] data,[MarshalAs(UnmanagedType.LPArray)]byte[] dst);

		[DllImport("g729")]
		private extern static void va_g729a_init_decoder();
		[DllImport("g729")]
		private extern static void va_g729a_decoder(byte[] data,byte[] dst,int bfi);
		public G729()
		{
		}
		public void InitalizeEncode()
		{
			va_g729a_init_encoder();
		}
		public void InitalizeDecode()
		{
			va_g729a_init_decoder();
		}
		unsafe public byte[] Encode(byte[] data)//采用Voiceage公司-G.729编码
		{
			MemoryStream src=new MemoryStream(data);
			System.IO.BinaryReader brsrc=new BinaryReader(src);
			MemoryStream dst=new MemoryStream();
			System.IO.BinaryWriter bwdst=new BinaryWriter(dst);
			int step=(int)(data.Length/160);
			for(int i=0;i<step;i++)
			{
				byte[] d=new byte[10];
				for(int k=0;k<d.Length;k++)
				{
					d[k]=1;
				}
				byte[] o=brsrc.ReadBytes(160);
				va_g729a_encoder(o,d);
				
				bwdst.Write(d);
			}
			byte[] ret=dst.GetBuffer();
			brsrc.Close();
			bwdst.Close();
			src.Close();
			dst.Close();
			return ret;		
		}
		public byte[] Decode(byte[] data)//Voiceage公司-G.729解码
		{
			MemoryStream src=new MemoryStream(data);
			System.IO.BinaryReader brsrc=new BinaryReader(src);
			MemoryStream dst=new MemoryStream();
			System.IO.BinaryWriter bwdst=new BinaryWriter(dst);
			int step=(int)(data.Length/10);
			for(int i=0;i<step;i++)
			{
				byte[] d=new byte[160];
				va_g729a_decoder(brsrc.ReadBytes(10),d,0);
				bwdst.Write(d);
			}
			byte[] ret=dst.GetBuffer();
			brsrc.Close();
			bwdst.Close();
			src.Close();
			dst.Close();
			return ret;			
		}

	}
}
