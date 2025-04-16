using System;

#nullable disable
namespace XRL.World.Parts.Mutation
{
    [Serializable]
    public class Brothers_Tortoises_CryoGasGeneration : GasGeneration
    {
        public Brothers_Tortoises_CryoGasGeneration()
          : base("CryoGas")
        {
        }

        public override int GetReleaseDuration(int Level) => Level + 2;

        public override int GetReleaseCooldown(int Level) => 40;

        public override string GetReleaseAbilityName() => "Release Cryogenic Mist";
    }
}