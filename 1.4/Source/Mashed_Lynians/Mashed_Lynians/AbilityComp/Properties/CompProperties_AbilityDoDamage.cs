﻿using Verse;
using RimWorld;

namespace Mashed_Lynians
{
    public class CompProperties_AbilityDoDamage : CompProperties_AbilityEffect
    {
        public DamageDef damageDef;
        public float damageAmount = 10f;
        public bool onlyHostile = false;

        public CompProperties_AbilityDoDamage()
        {
            this.compClass = typeof(CompAbilityEffect_DoDamage);
        }
    }
}
