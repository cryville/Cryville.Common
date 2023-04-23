using System;
using System.IO;
using System.Text;

namespace Cryville.Common.IO {
	public class BinaryReaderBE : BinaryReader {
		readonly byte[] m_buffer = new byte[8];

		public BinaryReaderBE(Stream input) : base(input) { }

		public BinaryReaderBE(Stream input, Encoding encoding) : base(input, encoding) { }

		public BinaryReaderBE(Stream input, Encoding encoding, bool leaveOpen) : base(input, encoding, leaveOpen) { }

		public override short ReadInt16() {
			FillBuffer(2);
			return (short)(m_buffer[1] | (m_buffer[0] << 8));
		}
		public override ushort ReadUInt16() {
			FillBuffer(2);
			return (ushort)(m_buffer[1] | (m_buffer[0] << 8));
		}

		public override int ReadInt32() {
			FillBuffer(4);
			return m_buffer[3] | (m_buffer[2] << 8) | (m_buffer[1] << 16) | (m_buffer[0] << 24);
		}
		public override uint ReadUInt32() {
			FillBuffer(4);
			return (uint)(m_buffer[3] | (m_buffer[2] << 8) | (m_buffer[1] << 16) | (m_buffer[0] << 24));
		}

		public override long ReadInt64() {
			FillBuffer(8);
			uint num = (uint)(m_buffer[7] | (m_buffer[6] << 8) | (m_buffer[5] << 16) | (m_buffer[4] << 24));
			uint num2 = (uint)(m_buffer[3] | (m_buffer[2] << 8) | (m_buffer[1] << 16) | (m_buffer[0] << 24));
			return (long)(((ulong)num2 << 32) | num);
		}
		public override ulong ReadUInt64() {
			FillBuffer(8);
			uint num = (uint)(m_buffer[7] | (m_buffer[6] << 8) | (m_buffer[5] << 16) | (m_buffer[4] << 24));
			uint num2 = (uint)(m_buffer[3] | (m_buffer[2] << 8) | (m_buffer[1] << 16) | (m_buffer[0] << 24));
			return ((ulong)num2 << 32) | num;
		}
		protected new void FillBuffer(int numBytes) {
			if (m_buffer != null && (numBytes < 0 || numBytes > m_buffer.Length)) {
				throw new ArgumentOutOfRangeException("numBytes", "Requested numBytes is larger than the internal buffer size");
			}

			int num = 0, num2;
			if (BaseStream == null) {
				throw new IOException("File not open");
			}

			if (numBytes == 1) {
				num2 = BaseStream.ReadByte();
				if (num2 == -1) {
					throw new EndOfStreamException("The end of the stream is reached before numBytes could be read");
				}
				m_buffer[0] = (byte)num2;
				return;
			}

			do {
				num2 = BaseStream.Read(m_buffer, num, numBytes - num);
				if (num2 == 0) {
					throw new EndOfStreamException("The end of the stream is reached before numBytes could be read");
				}

				num += num2;
			}
			while (num < numBytes);
		}
	}
}
