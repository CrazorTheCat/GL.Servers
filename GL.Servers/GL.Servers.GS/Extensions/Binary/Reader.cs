﻿namespace GL.Servers.GS.Extensions.Binary
{
    using System;
    using System.IO;
    using System.Text;

    using GL.Servers.GS.Files;
    using GL.Servers.GS.Files.CSV_Helpers;

    public class Reader : BinaryReader
    {
        public Reader(byte[] _Buffer) : base(new MemoryStream(_Buffer))
        {
            // Packet Reader...
        }

        /// <summary>
        /// Gets a value indicating whether [end of stream].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [end of stream]; otherwise, <c>false</c>.
        /// </value>
        public bool EndOfStream
        {
            get
            {
                return this.BaseStream.Length == this.BaseStream.Position;
            }
        }

        /// <summary>
        /// Reads the specified buffer.
        /// </summary>
        /// <param name="_Buffer">The buffer.</param>
        /// <param name="_Offset">The offset.</param>
        /// <param name="_Count">The count.</param>
        /// <returns></returns>
        public override int Read(byte[] _Buffer, int _Offset, int _Count)
        {
            return this.BaseStream.Read(_Buffer, 0, _Count);
        }

        /// <summary>
        /// Reads the byte array.
        /// </summary>
        /// <returns></returns>
        public byte[] ReadArray()
        {
            int _Length = this.ReadInt32();
            if (_Length == -1 || _Length < -1 || _Length > this.BaseStream.Length - this.BaseStream.Position)
            {
                return null;
            }

            byte[] _Buffer = this.ReadBytesWithEndian(_Length, false);
            return _Buffer;
        }

        /// <summary>
        /// Reads a Boolean value from the current stream and advances the current position of the stream by one byte.
        /// </summary>
        /// <returns>
        /// true if the byte is nonzero; otherwise, false.
        /// </returns>
        /// <exception cref="System.Exception">Error when reading a bool in packet.</exception>
        public override bool ReadBoolean()
        {
            byte state = this.ReadByte();
            switch (state)
            {
                case 0:
                    return false;

                case 1:
                    return true;

                default:
                    throw new NotSupportedException("Attempted to read a byte [" + state + "] but and convert it to boolean, but is out of boolean range.");
            }
        }

        /// <summary>
        /// Reads the next byte from the current stream and advances the current position of the stream by one byte.
        /// </summary>
        /// <returns>
        /// The next byte read from the current stream.
        /// </returns>
        public override byte ReadByte()
        {
            return (byte) this.BaseStream.ReadByte();
        }

        /// <summary>
        /// Reads the bytes.
        /// </summary>
        /// <returns></returns>
        public byte[] ReadBytes()
        {
            int length = this.ReadInt32();
            if (length == -1)
            {
                return null;
            }

            return this.ReadBytes(length);
        }

        /// <summary>
        /// Reads a 2-byte signed integer from the current stream and advances the current position of the stream by two bytes.
        /// </summary>
        /// <returns>
        /// A 2-byte signed integer read from the current stream.
        /// </returns>
        public override short ReadInt16()
        {
            return (short) this.ReadUInt16();
        }

        /// <summary>
        /// Reads the 3 bytes integer.
        /// </summary>
        /// <returns></returns>
        public int ReadInt24()
        {
            byte[] _Temp = this.ReadBytesWithEndian(3, false);
            return (_Temp[0] << 16) | (_Temp[1] << 8) | _Temp[2];
        }

        /// <summary>
        /// Reads a 4-byte signed integer from the current stream and advances the current position of the stream by four bytes.
        /// </summary>
        /// <returns>
        /// A 4-byte signed integer read from the current stream.
        /// </returns>
        public override int ReadInt32()
        {
            return (int) this.ReadUInt32();
        }

        /// <summary>
        /// Reads an 8-byte signed integer from the current stream and advances the current position of the stream by eight bytes.
        /// </summary>
        /// <returns>
        /// An 8-byte signed integer read from the current stream.
        /// </returns>
        public override long ReadInt64()
        {
            return (long) this.ReadUInt64();
        }

        /// <summary>
        /// Reads a string from the current stream. The string is prefixed with the length, encoded as an integer seven bits at a time.
        /// </summary>
        /// <returns>
        /// The string being read.
        /// </returns>
        public override string ReadString()
        {
            int _Length = this.ReadInt32();
            if (_Length == -1 || _Length < -1 || _Length > this.BaseStream.Length - this.BaseStream.Position)
            {
                return null;
            }

            byte[] _Buffer = this.ReadBytesWithEndian(_Length, false);
            return Encoding.UTF8.GetString(_Buffer);
        }

        /// <summary>
        /// Reads a 2-byte unsigned integer from the current stream using little-endian encoding and advances the position of the stream by two bytes.
        /// </summary>
        /// <returns>
        /// A 2-byte unsigned integer read from this stream.
        /// </returns>
        public override ushort ReadUInt16()
        {
            byte[] _Buffer = this.ReadBytesWithEndian(2);
            return BitConverter.ToUInt16(_Buffer, 0);
        }

        public uint ReadUInt24()
        {
            return (uint) this.ReadInt24();
        }

        /// <summary>
        /// Reads a 4-byte unsigned integer from the current stream and advances the position of the stream by four bytes.
        /// </summary>
        /// <returns>
        /// A 4-byte unsigned integer read from this stream.
        /// </returns>
        public override uint ReadUInt32()
        {
            byte[] _Buffer = this.ReadBytesWithEndian(4);
            return BitConverter.ToUInt32(_Buffer, 0);
        }

        /// <summary>
        /// Reads an 8-byte unsigned integer from the current stream and advances the position of the stream by eight bytes.
        /// </summary>
        /// <returns>
        /// An 8-byte unsigned integer read from this stream.
        /// </returns>
        public override ulong ReadUInt64()
        {
            byte[] _Buffer = this.ReadBytesWithEndian(8);
            return BitConverter.ToUInt64(_Buffer, 0);
        }

        /// <summary>
        /// Seeks to the specified offset.
        /// </summary>
        /// <param name="_Offset">The offset.</param>
        /// <param name="_Origin">The origin.</param>
        /// <returns></returns>
        public long Seek(long _Offset, SeekOrigin _Origin = SeekOrigin.Current)
        {
            return this.BaseStream.Seek(_Offset, _Origin);
        }

        /// <summary>
        /// Reads the v int.
        /// </summary>
        public int ReadVInt()
        {
            byte b = this.ReadByte();
            int v5 = b & 0x80;
            int _LR = b & 0x3F;

            if ((b & 0x40) != 0)
            {
                if (v5 != 0)
                {
                    b = this.ReadByte();
                    v5 = ((b << 6) & 0x1FC0) | _LR;

                    if ((b & 0x80) != 0)
                    {
                        b = this.ReadByte();
                        v5 = v5 | ((b << 13) & 0xFE000);

                        if ((b & 0x80) != 0)
                        {
                            b = this.ReadByte();
                            v5 = v5 | ((b << 20) & 0x7F00000);

                            if ((b & 0x80) != 0)
                            {
                                b = this.ReadByte();
                                _LR = (int) (v5 | (b << 27) | 0x80000000);
                            }
                            else
                            {
                                _LR = (int) (v5 | 0xF8000000);
                            }
                        }
                        else
                        {
                            _LR = (int) (v5 | 0xFFF00000);
                        }
                    }
                    else
                    {
                        _LR = (int) (v5 | 0xFFFFE000);
                    }
                }
            }
            else
            {
                if (v5 != 0)
                {
                    b = this.ReadByte();
                    _LR |= (b << 6) & 0x1FC0;

                    if ((b & 0x80) != 0)
                    {
                        b = this.ReadByte();
                        _LR |= (b << 13) & 0xFE000;

                        if ((b & 0x80) != 0)
                        {
                            b = this.ReadByte();
                            _LR |= (b << 20) & 0x7F00000;

                            if ((b & 0x80) != 0)
                            {
                                b = this.ReadByte();
                                _LR |= b << 27;
                            }
                        }
                    }
                }
            }

            return _LR;
        }

        /// <summary>
        /// Reads the v unsigned int.
        /// </summary>
        public uint ReadVUInt() 
        {
            return (uint) this.ReadVInt();
        }

        private byte[] ReadBytesWithEndian(int _Count, bool _Endian = true)
        {
            byte[] _Buffer = new byte[_Count];
            this.BaseStream.Read(_Buffer, 0, _Count);

            if (BitConverter.IsLittleEndian && _Endian)
            {
                Array.Reverse(_Buffer);
            }

            return _Buffer;
        }

        /// <summary>
        /// Reads the data.
        /// </summary>
        internal Data ReadData()
        {
            int Reference = this.ReadVInt();
            int RowIndex = this.ReadVInt();

            DataTable Table = CSV.Tables.Get(Reference);
            Data Data = Table.GetDataWithID(RowIndex);

            return Data;
        }
    }
}