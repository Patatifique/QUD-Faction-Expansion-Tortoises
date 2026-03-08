using System;

#nullable disable
namespace XRL.World.Quests
{
    [Serializable]
    public class Brothers_Tortoises_CombingTheSaltDunesSystem : IQuestSystem
    {
        public override void Start()
        {
            if (!The.Game.HasQuest("Brothers_TortoisesWardenQuest"))
                return;
            The.Game.FinishQuestStep("Brothers_TortoisesPoacherQuest", "Visit");
        }

        public override GameObject GetInfluencer() =>
          GameObject.FindByBlueprint("Brothers_Tortoises_Cracks-the-Saltback-Shell");
    }
}
