using System;
using HarmonyLib;
using PipeSystem;
using Verse;
using System.Reflection;

namespace VNPE
{
    [HarmonyPatch(typeof(CompConvertToThing), "MaxCanOutput", MethodType.Getter)]
    public static class CompConvertToThing_MaxCanOutput
    {
        static bool Prefix(CompConvertToThing __instance, ref int __result)
        {
            if (__instance.parent.def.defName == "VNPE_NutrientPasteFeeder" && __instance.parent is Building_Feeder feeder)
            {
                int current = 0;
                var container = feeder.InnerContainer;
                for (int i = 0; i < container.Count; i++)
                {
                    var t = container[i];
                    if (t.def == __instance.Props.thing)
                        current += t.stackCount;
                }
                var field = AccessTools.Field(typeof(CompConvertToThing), "maxHeldThingStackSize");
                int maxHeld = (int)field.GetValue(__instance);
                int limit = __instance.Props.maxOutputStackSize != -1 ? Math.Min(__instance.Props.maxOutputStackSize, __instance.Props.thing.stackLimit) : Math.Min(maxHeld, __instance.Props.thing.stackLimit);
                int max = Math.Min(limit, maxHeld);
                int remaining = max - current;
                __result = remaining > 0 ? remaining : 0;
                return false;
            }
            return true;
        }
    }
}
