// -----------------------------------------------------------------------
// <copyright file="ScoreboardPacket.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Starboard.Sockets
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Starboard.Sockets.Commands;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScoreboardPacket
    {
        public static byte MagicNumber
        {
            get
            {
                return 0x30;
            }
        }

        public IScoreboardPacketCommand[] Commands { get; set; }

        public static ScoreboardPacket Parse(byte[] data)
        {
            using (Stream stream = new MemoryStream(data))
            {
                using (var reader = new BinaryReader(stream))
                {
                    var magicNumber = reader.ReadByte();
                    if (magicNumber != MagicNumber)
                    {
                        throw new NotSupportedException("Sanity check for received packet was not valid.");
                    }

                    byte numCommands = reader.ReadByte();

                    var commands = new List<IScoreboardPacketCommand>();

                    for (int i = 0; i < numCommands; i++)
                    {
                        var pt = (PacketCommandType)reader.ReadByte();

                        switch (pt)
                        {
                            case PacketCommandType.EmptyCommand:
                                {
                                    var cmd = EmptyPacketCommand.Parse(reader);
                                    commands.Add(cmd);
                                    break;
                                }

                            case PacketCommandType.ByteCommand:
                                {
                                    var cmd = BytePacketCommand.Parse(reader);
                                    commands.Add(cmd);
                                    break;
                                }

                            case PacketCommandType.Int32Command:
                                {
                                    var cmd = Int32PacketCommand.Parse(reader);
                                    commands.Add(cmd);
                                    break;
                                }

                            case PacketCommandType.StringCommand:
                                {
                                    var cmd = StringPacketCommand.Parse(reader);
                                    commands.Add(cmd);
                                    break;
                                }
                        }
                    }

                    var packet = new ScoreboardPacket { Commands = commands.ToArray() };

                    return packet;
                }
            }
        }

        public byte[] ToBytes()
        {
            var buffer = new List<byte> { MagicNumber };

            var numCommands = (byte)this.Commands.Length;
            buffer.Add(numCommands);

            foreach (var command in this.Commands)
            {
                buffer.AddRange(command.ToBytes());
            }

            return buffer.ToArray();
        }
    }
}
