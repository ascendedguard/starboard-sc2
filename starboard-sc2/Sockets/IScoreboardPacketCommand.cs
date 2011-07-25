// -----------------------------------------------------------------------
// <copyright file="IScoreboardPacketCommand.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Starboard.Sockets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IScoreboardPacketCommand
    {
        PacketCommandType PacketType { get; }

        CommandType Command { get; }

        byte[] ToBytes();
    }
}
