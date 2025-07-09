using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;

namespace VNPE
{
    [HarmonyPatch(typeof(JobGiver_GetFood))]
    [HarmonyPatch("TryGiveJob")]
    public static class JobGiver_GetFood_TryGiveJob
    {
        private static readonly Dictionary<Pawn, int> lastTick = new();

        public static bool Prefix(Pawn pawn, ref Job __result)
        {
            if (pawn.InMentalState || !pawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
            {
                __result = null;
                return false;
            }

            int currentTick = Find.TickManager.TicksGame;
            if (lastTick.TryGetValue(pawn, out var tick) && tick == currentTick)
            {
                __result = null;
                return false;
            }

            lastTick[pawn] = currentTick;
            return true;
        }
    }
}
