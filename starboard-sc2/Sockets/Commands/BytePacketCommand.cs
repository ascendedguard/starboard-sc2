// -----------------------------------------------------------------------
// <copyright file="BytePacketCommand.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Starboard.Sockets.Commands
{
    using System;
    using System.IO;

    /// <summary>
    /// Defines a command with a single byte of data.
    /// </summary>
    public class BytePacketCommand : ScoreboardPacketCommandBase
    {
        public override PacketCommandType PacketType
        {
            get
            {
                return PacketCommandType.ByteCommand;
            }
        }

        public byte Data { get; set; }
            
        public override byte[] ToBytes()
        {
            var pt = (byte)this.PacketType;
            var cmd = (byte)this.Command;
            
            return new[] { pt, cmd, this.Player, this.Data };
        }

        public static BytePacketCommand Parse(BinaryReader reader)
        {
            var cmd = reader.ReadByte();
            var player = reader.ReadByte();
            var data = reader.ReadByte();

            return new BytePacketCommand { Command = (CommandType)cmd, Player = player, Data = data };
        }
    }
}
