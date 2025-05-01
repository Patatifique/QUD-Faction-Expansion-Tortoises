using System;

#nullable disable
namespace XRL.World.Quests
{
    [Serializable]
    public class CombingTheSaltDunesSystem : IQuestSystem
    {
        public override void Start()
        {
            ZoneManager.instance.GetZone("JoppaWorld").BroadcastEvent("SaltbackGraveyardReveal");
            if (!The.Game.HasQuest("The Scute Child"))
                return;
            The.Game.FinishQuestStep("Combing the Salt Dunes", "Locate the Saltback Graveyard");
        }

        public override GameObject GetInfluencer() =>
          GameObject.FindByBlueprint("Brothers_Tortoises_Cracks-the-Saltback-Shell");
    }
}
