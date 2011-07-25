// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PacketCommandType.cs" company="Starboard">
//   Copyright © 2011 All Rights Reserved
// </copyright>
// <summary>
//   Defines the type of packet to be expected from the following IScoreboardPacketCommand, defining
//   the number of bytes that will be read.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Starboard.Sockets
{
    /// <summary>
    /// Defines the type of packet to be expected from the following IScoreboardPacketCommand, defining
    /// the number of bytes that will be read.
    /// </summary>
    public enum PacketCommandType
    {
        /// <summary>
        /// Defines a command that has no data beyond the command type.
        /// </summary>
        EmptyCommand = 0x00,

        /// <summary>
        /// Defines a command that has an Int32 length, followed by a string, using UTF8 encoding.
        /// </summary>
        StringCommand = 0x01,

        /// <summary>
        /// Defines a command that has 4 bytes of data beyond the command type.
        /// </summary>
        Int32Command = 0x02,
        
        /// <summary>
        /// Defines a command that has a single byte of data beyond the command type.
        /// </summary>
        ByteCommand = 0x03,        
    }
}
