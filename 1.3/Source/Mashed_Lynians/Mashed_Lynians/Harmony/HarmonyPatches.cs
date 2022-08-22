﻿using HarmonyLib;
using System.Text;
using Verse;
using System.Reflection;
using RimWorld;

namespace Mashed_Lynians
{
    [StaticConstructorOnStartup]
    public class Main
    {
        static Main()
        {
            var harmony = new Harmony("com.Mashed_Lynians");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }

    /// <summary>
    /// Just adds hediffs to pawns when they are generated.
    /// </summary>
    [HarmonyPatch(typeof(PawnGenerator))]
    [HarmonyPatch("GenerateInitialHediffs")]
    public static class PawnGenerator_GenerateInitialHediffs_Patch
    {
        [HarmonyPostfix]
        public static void LyniansPatch(Pawn pawn)
        {
            RaceProperties props = RaceProperties.Get(pawn.def);
            if (props != null && !props.hediffsToAdd.NullOrEmpty())
            {
                foreach(HediffDef hd in props.hediffsToAdd)
                {
                    pawn.health.AddHediff(hd);
                }
            }
        }
    }

    /// <summary>
    /// Replaces potential Felvine addiction with Alcohol addiction during pawn generation, for non-cat-like lynians.
    /// </summary>
    [HarmonyPatch(typeof(PawnAddictionHediffsGenerator))]
    [HarmonyPatch("ApplyAddiction")]
    public static class PawnAddictionHediffsGenerator_ApplyAddiction_Patch
    {
        [HarmonyPrefix]
        public static void LyniansPatch(Pawn pawn, ref ChemicalDef chemicalDef)
        {
            RaceProperties props = RaceProperties.Get(pawn.def);
            if (chemicalDef == ChemicalDefOf.Mashed_Lynian_FelvineChemical && (props == null || !props.canUseFelvine))
            {
                chemicalDef = RimWorld.ChemicalDefOf.Alcohol;
            }
        }
    }

    /// <summary>
    /// Allows for backpacks that increase carry weight in caravans.
    /// </summary>
    [HarmonyPatch(typeof(MassUtility))]
    [HarmonyPatch("Capacity")]
    public static class MassUtility_Capacity_Patch
    {
        [HarmonyPostfix]
        public static void LyniansPatch(ref float __result, Pawn p, StringBuilder explanation = null)
        {
            if(p != null && p.apparel != null && p.apparel.WornApparel != null)
            {
                foreach(Apparel ap in p.apparel.WornApparel)
                {
                    ApparelProperties props = ApparelProperties.Get(ap.def);
                    if (props != null && props.carryWeightIncrease != 0)
                    {
                        float additional = props.carryWeightIncrease;
                        if (!props.qualityCarryWeightMults.NullOrEmpty())
                        {
                            ap.TryGetQuality(out QualityCategory qc);
                            additional *= props.qualityCarryWeightMults[(int)qc];
                        }
                        __result += additional;
                    }
                }
            }
        }
    }
}
