// -----------------------------------------------------------------------
// <copyright file="BytePacketCommand.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Starboard.Sockets.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Defines a command with a single byte of data.
    /// </summary>
    public class Int32PacketCommand : ScoreboardPacketCommandBase
    {
        public override PacketCommandType PacketType
        {
            get
            {
                return PacketCommandType.Int32Command;
            }
        }

        public int Data { get; set; }

        public override byte[] ToBytes()
        {
            var output = new List<byte>();

            var pt = (byte)this.PacketType;
            var cmd = (byte)this.Command;

            var data = BitConverter.GetBytes(this.Data);

            output.Add(pt);
            output.Add(cmd);
            output.Add(this.Player);
            output.AddRange(data);

            return output.ToArray();
        }

        public static Int32PacketCommand Parse(BinaryReader reader)
        {
            var cmd = reader.ReadByte();
            var player = reader.ReadByte();
            var dataBytes = reader.ReadBytes(4);

            var data = BitConverter.ToInt32(dataBytes, 0);

            return new Int32PacketCommand { Command = (CommandType)cmd, Player = player, Data = data };
        }
    }
}
