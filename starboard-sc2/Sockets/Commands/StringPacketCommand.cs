// -----------------------------------------------------------------------
// <copyright file="StringPacketCommand.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Starboard.Sockets.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class StringPacketCommand : ScoreboardPacketCommandBase
    {
        public override PacketCommandType PacketType
        {
            get
            {
                return PacketCommandType.StringCommand;
            }
        }

        public string Data { get; set; }

        public override byte[] ToBytes()
        {
            var output = new List<byte>();

            var pt = (byte)this.PacketType;
            var cmd = (byte)this.Command;

            var data = Encoding.UTF8.GetBytes(this.Data);
            var count = BitConverter.GetBytes(data.Length);

            output.Add(pt);
            output.Add(cmd);
            output.Add(this.Player);
            output.AddRange(count);
            output.AddRange(data);

            return output.ToArray();
        }

        public static StringPacketCommand Parse(BinaryReader reader)
        {
            var cmd = reader.ReadByte();
            var player = reader.ReadByte();
            var lengthBytes = reader.ReadBytes(4);

            var length = BitConverter.ToInt32(lengthBytes, 0);

            var dataBytes = reader.ReadBytes(length);
            var data = Encoding.UTF8.GetString(dataBytes);

            return new StringPacketCommand { Command = (CommandType)cmd, Player = player, Data = data };
        }
    }
}
