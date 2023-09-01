using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static PrivilegeManager;

namespace ImprovedSmelters
{
    public class PluginInfo
    {
        public const string Name = "Improved Smelters";
        public const string Guid = "beardedkwan.ImprovedSmelters";
        public const string Version = "1.2.1";
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

        // add all
        public static ConfigEntry<bool> EnableAddAll { get; set; }
        public static ConfigEntry<string> UseKey { get; set; }

    }

    [BepInPlugin(PluginInfo.Guid, PluginInfo.Name, PluginInfo.Version)]
    [BepInProcess("valheim.exe")]
    public class ImprovedSmelters : BaseUnityPlugin
    {
        void Awake()
        {
            // INITIALIZE CONFIG

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

            // add all
            ImprovedSmeltersConfig.EnableAddAll = Config.Bind("Add All", "EnableAddAll", true, "Flag to enable add all functionality for smelters.\nThis will fill the smelter from your inventory when you use Shift+F to load the smelter.");
            ImprovedSmeltersConfig.UseKey = Config.Bind("Add All", "UseKey", "F", "Shift + [UseKey] to use add all for smelters");

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

        // ADD ALL PATCHES

        // Fuel hover patch
        [HarmonyPatch(typeof(Smelter), "OnHoverAddFuel")]
        public static class OnHoverAddFuelPatch
        {
            private static void Postfix(ref String __result)
            {
                if (ImprovedSmeltersConfig.EnableAddAll.Value)
                {
                    __result = __result + "\n[<color=yellow><b>Shift + F</b></color>] Add All";
                }
            }
        }

        // Add fuel patch
        [HarmonyPatch(typeof(Smelter), "OnAddFuel")]
        private static class OnAddFuelPatch
        {
            private static bool Prefix(Smelter __instance, ref bool __result, Humanoid user, ZNetView ___m_nview)
            {
                // Check if Shift key is held down and F is pressed
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.F) && ImprovedSmeltersConfig.EnableAddAll.Value)
                {
                    //UnityEngine.Debug.Log("Hit Shift+F OnAddFuel (Adding all fuel)");

                    float fuelInSmelter = Traverse.Create(__instance).Method("GetFuel").GetValue<float>();
                    float fuelToAdd = __instance.m_maxFuel - fuelInSmelter;

                    Inventory playerInventory = user.GetInventory();
                    ItemDrop.ItemData item = playerInventory.GetItem(__instance.m_fuelItem.m_itemData.m_shared.m_name);

                    if (fuelToAdd > 0 && ___m_nview != null && item != null)
                    {
                        if (fuelToAdd > item.m_stack)
                        {
                            fuelToAdd = item.m_stack;
                        }

                        user.GetInventory().RemoveItem(item.m_shared.m_name, (int)fuelToAdd);

                        for (int i = 0; i < fuelToAdd; i++)
                        {
                            ___m_nview.InvokeRPC("AddFuel");
                        }
                    }

                    //UnityEngine.Debug.Log("Added all fuel");

                    __result = false; // Return false to prevent the original method from executing
                    return false;
                }

                // Call the original method if the Shift+F combination is not detected
                return true;
            }
        }

        // Ore hover patch
        [HarmonyPatch(typeof(Smelter), "OnHoverAddOre")]
        public static class OnHoverAddOrePatch
        {
            private static void Postfix(ref String __result)
            {
                if (ImprovedSmeltersConfig.EnableAddAll.Value)
                {
                    __result = __result + "\n[<color=yellow><b>Shift + F</b></color>] Add All";
                }
            }
        }

        // Add ore patch
        [HarmonyPatch(typeof(Smelter), "OnAddOre")]
        private static class OnAddOre
        {
            private static bool Prefix(Smelter __instance, ref bool __result, Humanoid user, ZNetView ___m_nview)
            {
                // Check if Shift key is held down and [UseKey] is pressed
                string keyUse = ImprovedSmeltersConfig.UseKey.Value;
                if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown((KeyCode)Enum.Parse(typeof(KeyCode), keyUse)) && ImprovedSmeltersConfig.EnableAddAll.Value)
                {
                    //UnityEngine.Debug.Log("Hit Shift+F OnAddFuel (Adding all ore)");

                    float oreInSmelter = Traverse.Create(__instance).Method("GetQueueSize").GetValue<int>();
                    float oreToAdd = __instance.m_maxOre - oreInSmelter;

                    Inventory playerInventory = user.GetInventory();
                    ItemDrop.ItemData item = Traverse.Create(__instance).Method("FindCookableItem", user.GetInventory()).GetValue<ItemDrop.ItemData>();

                    if (oreToAdd > 0 && ___m_nview != null && item != null)
                    {
                        if (oreToAdd > item.m_stack)
                        {
                            oreToAdd = item.m_stack;
                        }

                        user.GetInventory().RemoveItem(item, (int)oreToAdd);

                        for (int i = 0; i < oreToAdd; i++)
                        {
                            ___m_nview.InvokeRPC("AddOre", item.m_dropPrefab.name);
                        }
                    }

                    //UnityEngine.Debug.Log("Added all ore");

                    __result = false; // Return false to prevent the original method from executing
                    return false;
                }

                // Call the original method if the Shift+F combination is not detected
                return true;
            }
        }
    }
}
