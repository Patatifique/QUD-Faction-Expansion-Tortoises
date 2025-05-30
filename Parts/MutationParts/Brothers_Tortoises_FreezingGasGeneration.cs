using System;

#nullable disable
namespace XRL.World.Parts.Mutation
{
    [Serializable]
    public class Brothers_Tortoises_FreezingGasGeneration : GasGeneration
    {
        public Brothers_Tortoises_FreezingGasGeneration()
          : base("Brothers_Tortoises_FreezingGas")
        {
        }

        public override int GetReleaseDuration(int Level) => Level + 2;

        public override int GetReleaseCooldown(int Level) => 40;

        public override string GetReleaseAbilityName() => "Release Freezing Gas";
    }
}