using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImprovedSmelters
{
    public class PluginInfo
    {
        public const string Name = "Improved Smelters";
        public const string Guid = "beardedkwan.ImprovedSmelters";
        public const string Version = "1.0.0";
    }

    [BepInPlugin(PluginInfo.Guid, PluginInfo.Name, PluginInfo.Version)]
    [BepInProcess("valheim.exe")]
    public class ImprovedSmelters : BaseUnityPlugin
    {
        void Awake()
        {
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
                //Debug.Log($"smelter patch, name: '{name}'");

                if (name.StartsWith("charcoal_kiln"))
                {
                    __instance.m_maxOre = 30;
                    __instance.m_secPerProduct = 3f;
                }
                else if (name.StartsWith("smelter"))
                {
                    __instance.m_maxOre = 30;
                    __instance.m_maxFuel = 30;
                    __instance.m_fuelPerProduct = 1;
                    __instance.m_secPerProduct = 3f;
                }
                else if (name.StartsWith("blastfurnace"))
                {
                    __instance.m_maxOre = 30;
                    __instance.m_maxFuel = 30;
                    __instance.m_fuelPerProduct = 1;
                    __instance.m_secPerProduct = 3f;
                }
                else if (name.StartsWith("piece_spinningwheel"))
                {
                    __instance.m_maxOre = 50;
                    __instance.m_secPerProduct = 3f;
                }
                else if (name.StartsWith("windmill"))
                {
                    __instance.m_maxOre = 50;
                    __instance.m_secPerProduct = 3f;
                }
                else if (name.StartsWith("eitrrefinery"))
                {
                    __instance.m_maxOre = 30;
                    __instance.m_maxFuel = 30;
                    __instance.m_secPerProduct = 3f;
                }
            }
        }
    }
}
