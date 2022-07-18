﻿using Microsoft.Xna.Framework;
using TerraAngel.Hooks;
using TerraAngel.Graphics;
using TerraAngel.Input;
using TerraAngel.Client;
using TerraAngel.Client.Config;
using TerraAngel.Client.ClientWindows;
using System;
using System.IO;
using TerraAngel.Plugin;

namespace TerraAngel.Loader
{
    public class ClientLoader
    {
        public static ClientRenderer? MainRenderer;
        public static ConfigUI ConfigUI = new ConfigUI();
        public static PluginUI PluginUI = new PluginUI();
        internal static ConsoleWindow? ConsoleWindow;

        public static bool SetupRenderer = false;

        public static string SavePath => Path.Combine(Terraria.Main.SavePath, "TerraAngel");
        public static string ConfigPath => Path.Combine(SavePath, "clientConfig.json");
        public static string PluginsPath => Path.Combine(SavePath, "Plugins");

        public static void Hookgen_Early()
        {
            GameHooks.Generate();

            new ClientConfig();
        }

        public static void SetupImGuiRenderer(Game main)
        {
            if (!SetupRenderer)
            {
                SetupRenderer = true;
                MainRenderer = new ClientRenderer(main);
                ClientConfig.Instance.PluginsToEnable = ClientConfig.Instance.pluginsToEnable;
            }
        }

        public static class Console
        {
            public static void WriteLine(string message) => ConsoleWindow?.WriteLine(message);
            public static void WriteLine(string message, Color color) => ConsoleWindow?.WriteLine(message, color);
            public static void WriteError(string message) => ConsoleWindow?.WriteError(message);
            public static void AddCommand(string name, Action<ConsoleWindow.CmdStr> action, string description = "No Description Given") => ConsoleWindow?.AddCommand(name, action, description);
            public static void ClearConsole() => ConsoleWindow?.ClearConsole();
        }
    }
}
