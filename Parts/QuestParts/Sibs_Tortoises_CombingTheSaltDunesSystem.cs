using System;
using XRL.World.Parts;

#nullable disable
namespace XRL.World.Quests
{
    [Serializable]
    public class Sibs_Tortoises_CombingTheSaltDunesSystem : IQuestSystem
    {
        public override void Start()
        {
            if (!The.Game.HasQuest("Sibs_TortoisesWardenQuest"))
                return;
            The.Game.FinishQuestStep("Sibs_TortoisesPoacherQuest", "Visit");
        }


                public override void Finish()
        {
            // On Poacher ending
            if (The.Game.GetBooleanGameState("Sibs_Tortoises_PoacherEnding"))
            {   
                // Timer for consequences
                var timerPart = new Sibs_BoolStateTimer();
                timerPart.startTurn = Calendar.TotalTimeTicks;
                timerPart.state = "Sibs_Tortoises_PoacherEnding_Occured";
                timerPart.targetTurns = 6000L; // a few moments later
                The.Player.AddPart(timerPart);

                // Attach Outcome Part to the player
                var outcomePart = new Sibs_Tortoises_PoacherOutcome();    
                The.Player.AddPart(outcomePart);                                    
            }
        }
        
        public override GameObject GetInfluencer() =>
          GameObject.FindByBlueprint("Sibs_Tortoises_Cracks-the-Saltback-Shell");
    }
}
