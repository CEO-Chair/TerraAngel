using Microsoft.Xna.Framework;
using TerraAngel.Hooks;
using TerraAngel.Graphics;
using TerraAngel.Input;
using TerraAngel.Client;
using TerraAngel.Client.Config;
using TerraAngel.Client.ClientWindows;
using System;
using System.IO;
using TerraAngel.Plugin;
using Terraria;
using TerraAngel.Cheat;
using TerraAngel.Cheat.Cringes;
using System.Reflection;
using System.Linq;
using TerraAngel.UI;

namespace TerraAngel
{
    public class ClientLoader
    {
        public static bool SetupRenderer = false;
        public static ClientRenderer? MainRenderer;
        internal static ConsoleWindow? ConsoleWindow;
        public static ConfigUI ConfigUI = new ConfigUI();
        public static PluginUI PluginUI = new PluginUI();
        public static ResolutionUI ResolutionUI = new ResolutionUI();
        public static ClientConfig Config = new ClientConfig();

        public static string SavePath => Path.Combine(Main.SavePath, "TerraAngel");
        public static string ConfigPath => Path.Combine(SavePath, "clientConfig.json");
        public static string PluginsPath => Path.Combine(SavePath, "Plugins");

        public static void Hookgen_Early()
        {
            GameHooks.Generate();

            Config = ClientConfig.ReadFromFile();

            Type[] cringeTypes = typeof(Cringe).Assembly.GetTypes().Where(x =>
                                                                        !x.IsAbstract &&
                                                                        x.IsSubclassOf(typeof(Cringe)) &&
                                                                        x.GetConstructor(Array.Empty<Type>()) != null).ToArray();
            for (int i = 0; i < cringeTypes.Length; i++)
            {
                Type type = cringeTypes[i];
                CringeManager.AddCringe(type);
            }

            CringeManager.SortTabs();

            CringeManager.GetCringe<AntiHurtCringe>().Enabled = Config.DefaultAntiHurt;
            CringeManager.GetCringe<InfiniteManaCringe>().Enabled = Config.DefaultInfiniteMana;
            CringeManager.GetCringe<InfiniteMinionCringe>().Enabled = Config.DefaultInfiniteMinions;
            CringeManager.GetCringe<ESPTracersCringe>().Enabled = Config.DefaultESPTracers;
            CringeManager.GetCringe<ESPTracersCringe>().TracerColor = Config.TracerColor;
            CringeManager.GetCringe<HeldItemViewerCringe>().Enabled = Config.DefaultShowHeldItem;

            ESPBoxesCringe boxesCringe = CringeManager.GetCringe<ESPBoxesCringe>();
            boxesCringe.NPCBoxes = Config.DefaultNPCBoxes;
            boxesCringe.ProjectileBoxes = Config.DefaultProjectileBoxes;
            boxesCringe.PlayerBoxes = Config.DefaultPlayerESPBoxes;
            boxesCringe.LocalPlayerColor = Config.LocalBoxPlayerColor;
            boxesCringe.OtherPlayerColor = Config.OtherBoxPlayerColor;
            boxesCringe.NPCColor = Config.NPCBoxColor;
            boxesCringe.ProjectileColor = Config.ProjectileBoxColor;
        }

        public static void SetupImGuiRenderer(Game main)
        {
            if (!SetupRenderer)
            {
                SetupRenderer = true;
                MainRenderer = new ClientRenderer(main);
                Config.PluginsToEnable = Config.pluginsToEnable;
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
