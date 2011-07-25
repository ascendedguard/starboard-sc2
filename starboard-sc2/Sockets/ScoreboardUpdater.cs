// -----------------------------------------------------------------------
// <copyright file="ScoreboardUpdater.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Starboard.Sockets
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    using Starboard.Sockets.Commands;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScoreboardUpdater
    {
        public IPEndPoint EndPoint { get; set; }

        public void SendPacket(ScoreboardPacket packet)
        {
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
                    {
                        ReceiveTimeout = 1000 
                    })
            {
                socket.Connect(this.EndPoint);

                socket.Send(packet.ToBytes());
            }
        }

        public bool Ping()
        {
            var commands = new[] { new EmptyPacketCommand() { Command = CommandType.Ping } };

            var packet = new ScoreboardPacket { Commands = commands };

            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp)
            {
                ReceiveTimeout = 1000
            })
            {
                try
                {
                    socket.Connect(this.EndPoint);

                    socket.Send(packet.ToBytes());

                    var buffer = new byte[100];
                    socket.Receive(buffer);

                    var incoming = ScoreboardPacket.Parse(buffer);
                    return incoming.Commands[0].Command == CommandType.Pong;
                }
                catch (Exception)
                {
                    // If anything fails, return false.
                    return false;
                }
            }
        }
    }
}
