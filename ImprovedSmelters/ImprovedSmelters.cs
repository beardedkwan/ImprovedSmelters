using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ImprovedSmelters
{
    public class PluginInfo
    {
        public const string Name = "Improved Smelters";
        public const string Guid = "beardedkwan.ImprovedSmelters";
        public const string Version = "1.1.0";
    }

    public class ImprovedSmeltersConfig
    {
        // kiln
        public static ConfigEntry<int> KilnMaxOre { get; set; }
        public static ConfigEntry<float> KilnSecondsPerProduct { get; set; }

        // smelter
        public static ConfigEntry<int> SmelterMaxOre { get; set; }
        public static ConfigEntry<int> SmelterMaxFuel { get; set; }
        public static ConfigEntry<int> SmelterFuelPerProduct { get; set; }
        public static ConfigEntry<float> SmelterSecondsPerProduct { get; set; }

        // eitr refinery
        public static ConfigEntry<int> ERMaxOre { get; set; }
        public static ConfigEntry<int> ERMaxFuel { get; set; }
        public static ConfigEntry<float> ERSecondsPerProduct { get; set; }

        // spinning wheel
        public static ConfigEntry<int> SWMaxOre { get; set; }
        public static ConfigEntry<float> SWSecondsPerProduct { get; set; }

        // windmill
        public static ConfigEntry<int> WindmillMaxOre { get; set; }
        public static ConfigEntry<float> WindmillSecondsPerProduct { get; set; }

    }

    [BepInPlugin(PluginInfo.Guid, PluginInfo.Name, PluginInfo.Version)]
    [BepInProcess("valheim.exe")]
    public class ImprovedSmelters : BaseUnityPlugin
    {
        void Awake()
        {
            // Initialize config

            // kiln
            ImprovedSmeltersConfig.KilnMaxOre = Config.Bind("Kiln", "KilnMaxOre", 25, "Maximum wood for kiln");
            ImprovedSmeltersConfig.KilnSecondsPerProduct = Config.Bind("Kiln", "KilnSecondsPerProduct", 3f, "Seconds per product for kiln");

            // smelter/blast furnace
            ImprovedSmeltersConfig.SmelterMaxOre = Config.Bind("Smelter/Blast Furnace", "MaxSmelterOre", 25, "Maximum ore for smelter/blast furnace");
            ImprovedSmeltersConfig.SmelterMaxFuel = Config.Bind("Smelter/Blast Furnace", "MaxSmelterFuel", 25, "Maximum fuel for smelter/blast furnace");
            ImprovedSmeltersConfig.SmelterFuelPerProduct = Config.Bind("Smelter/Blast Furnace", "SmelterFuelPerProduct", 1, "Fuel per product for smelter/blast furnace");
            ImprovedSmeltersConfig.SmelterSecondsPerProduct = Config.Bind("Smelter/Blast Furnace", "SmelterSecondsPerProduct", 3f, "Seconds per product for smelter/blast furnace");

            // eitr refinery
            ImprovedSmeltersConfig.ERMaxOre = Config.Bind("Eitr Refinery", "ERMaxOre", 25, "Maximum ore for eitr refinery");
            ImprovedSmeltersConfig.ERMaxFuel = Config.Bind("Eitr Refinery", "ERMaxFuel", 25, "Maximum fuel for eitr refinery");
            ImprovedSmeltersConfig.ERSecondsPerProduct = Config.Bind("Eitr Refinery", "ERSecondsPerProduct", 3f, "Seconds per product for eitr refinery");

            // spinning wheel
            ImprovedSmeltersConfig.SWMaxOre = Config.Bind("Spinning Wheel", "SWMaxOre", 50, "Maximum items for spnning wheel");
            ImprovedSmeltersConfig.SWSecondsPerProduct = Config.Bind("Spinning Wheel", "SWSecondsPerProduct", 3f, "Seconds per product for spinning wheel");

            // windmill
            ImprovedSmeltersConfig.WindmillMaxOre = Config.Bind("Windmill", "WindmillMaxOre", 50, "Maximum items for windmill");
            ImprovedSmeltersConfig.SWSecondsPerProduct = Config.Bind("Windmill", "SWSecondsPerProduct", 3f, "Seconds per product for windmill");

            Harmony harmony = new Harmony(PluginInfo.Guid);
            harmony.PatchAll();
        }

        // IMPROVED SMELTERS PATCH
        [HarmonyPatch(typeof(Smelter), "Awake")]
        public static class Smelter_Patch
        {
            private static void Prefix(ref Smelter __instance)
            {
                string name = __instance.name;
                UnityEngine.Debug.Log($"SMELTER PATCH - instance name: '{name}'");

                if (name.StartsWith("charcoal_kiln"))
                {
                    __instance.m_maxOre = ImprovedSmeltersConfig.KilnMaxOre.Value;
                    __instance.m_secPerProduct = ImprovedSmeltersConfig.KilnSecondsPerProduct.Value;
                }
                else if (name.StartsWith("smelter") || name.StartsWith("blastfurnace"))
                {
                    __instance.m_maxOre = ImprovedSmeltersConfig.SmelterMaxOre.Value;
                    __instance.m_maxFuel = ImprovedSmeltersConfig.SmelterMaxFuel.Value;
                    __instance.m_fuelPerProduct = ImprovedSmeltersConfig.SmelterFuelPerProduct.Value;
                    __instance.m_secPerProduct = ImprovedSmeltersConfig.SmelterSecondsPerProduct.Value;
                }
                else if (name.StartsWith("eitrrefinery"))
                {
                    __instance.m_maxOre = ImprovedSmeltersConfig.ERMaxOre.Value;
                    __instance.m_maxFuel = ImprovedSmeltersConfig.ERMaxFuel.Value;
                    __instance.m_secPerProduct = ImprovedSmeltersConfig.ERSecondsPerProduct.Value;
                }
                else if (name.StartsWith("piece_spinningwheel"))
                {
                    __instance.m_maxOre = ImprovedSmeltersConfig.SWMaxOre.Value;
                    __instance.m_secPerProduct = ImprovedSmeltersConfig.SWSecondsPerProduct.Value;
                }
                else if (name.StartsWith("windmill"))
                {
                    __instance.m_maxOre = ImprovedSmeltersConfig.WindmillMaxOre.Value;
                    __instance.m_secPerProduct = ImprovedSmeltersConfig.WindmillSecondsPerProduct.Value;
                }
            }
        }
    }
}
