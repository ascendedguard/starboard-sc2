// -----------------------------------------------------------------------
// <copyright file="CommandType.cs" company="Microsoft">
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
    /// List of Command Types defining what type of command is in an IScoreboardPacketCommand
    /// </summary>
    public enum CommandType
    {
        UpdatePlayerName =     0x01,
        UpdatePlayerScore =    0x02,
        UpdatePlayerRace =     0x03,
        UpdatePlayerColor =    0x04,
        ResetPlayer =          0x05,
        ShowScoreboard =       0x10,
        HideScoreboard =       0x11,
        ToggleAnnouncement =   0x20,
        ToggleSubbar =         0x21,
        SwapPlayer =           0x22,

        IncrementPlayerScore = 0x30,

        RetrievePlayerInformation = 0x40,

        Ping =                 0x90,
        Pong =                 0x91,
    }
}
