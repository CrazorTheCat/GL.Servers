﻿namespace GL.Clients.BS.Packets
{
    using System;
    using System.Collections.Generic;

    using GL.Clients.BS.Packets.Messages.Client;
    using GL.Clients.BS.Packets.Messages.Server;

    internal static class Factory
    {
        /// <summary>
        /// The delimiter used to detect if x string is a call-command.
        /// </summary>
        internal const char Delimiter = '/';

        internal static readonly Dictionary<ushort, Type> Messages = new Dictionary<ushort, Type>();
        internal static readonly Dictionary<ushort, Type> Commands = new Dictionary<ushort, Type>();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            Factory.LoadMessages();
            Factory.LoadCommands();
        }

        /// <summary>
        /// Loads the game messages.
        /// </summary>
        private static void LoadMessages()
        {
            Factory.Messages.Add(10101, typeof(Authentification));
            Factory.Messages.Add(10107, typeof(Client_Capabilities));
            Factory.Messages.Add(10108, typeof(Keep_Alive));
            Factory.Messages.Add(10113, typeof(Get_Device_Token));

            Factory.Messages.Add(14102, typeof(Execute_Commands));
            Factory.Messages.Add(14109, typeof(Go_Home));
            Factory.Messages.Add(14113, typeof(Ask_Profile));

            Factory.Messages.Add(20103, typeof(Authentification_Failed));
            Factory.Messages.Add(20104, typeof(Authentification_OK));

            Factory.Messages.Add(20108, typeof(Keep_Alive_OK));

            Factory.Messages.Add(23456, typeof(Battle_Result));

            Factory.Messages.Add(24101, typeof(Own_Home_Data));
            Factory.Messages.Add(24104, typeof(Out_Of_Sync));

            Factory.Messages.Add(24113, typeof(Profile_Data));
            Factory.Messages.Add(24115, typeof(Server_Error));

            Factory.Messages.Add(24311, typeof(Clan_Stream));
            Factory.Messages.Add(24399, typeof(Clan_Info));

            Factory.Messages.Add(25892, typeof(Disconnected));
        }

        /// <summary>
        /// Loads the game commands.
        /// </summary>
        private static void LoadCommands()
        {

        }
    }
}