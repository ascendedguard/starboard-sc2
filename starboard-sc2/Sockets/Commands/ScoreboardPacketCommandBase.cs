// -----------------------------------------------------------------------
// <copyright file="ScoreboardPacketCommandBase.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Starboard.Sockets.Commands
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public abstract class ScoreboardPacketCommandBase : IScoreboardPacketCommand
    {
        public abstract PacketCommandType PacketType { get; }

        public CommandType Command { get; set; }

        public byte Player { get; set; }

        public abstract byte[] ToBytes();
    }
}
