// -----------------------------------------------------------------------
// <copyright file="EmptyPacketCommand.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Starboard.Sockets.Commands
{
    using System.IO;

    /// <summary>
    /// Defines a command with no necessary data.
    /// </summary>
    public class EmptyPacketCommand : ScoreboardPacketCommandBase
    {
        public override PacketCommandType PacketType
        {
            get
            {
                return PacketCommandType.EmptyCommand;
            }
        }

        public override byte[] ToBytes()
        {
            var pt = (byte)this.PacketType; 
            var cmd = (byte)this.Command;

            return new[] { pt, cmd, this.Player };
        }

        public static EmptyPacketCommand Parse(BinaryReader reader)
        {
            var cmd = reader.ReadByte();
            var player = reader.ReadByte();

            return new EmptyPacketCommand { Command = (CommandType)cmd, Player = player };
        }
    }
}
