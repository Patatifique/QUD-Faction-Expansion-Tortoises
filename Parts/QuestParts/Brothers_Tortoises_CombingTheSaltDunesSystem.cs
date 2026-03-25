using System;
using XRL.World.Parts;

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


                public override void Finish()
        {
            // On Poacher ending
            if (The.Game.GetBooleanGameState("Brothers_Tortoises_PoacherEnding"))
            {   
                // Timer for consequences
                var timerPart = new Brothers_BoolStateTimer();
                timerPart.startTurn = Calendar.TotalTimeTicks;
                timerPart.state = "Brothers_Tortoises_PoacherEnding_Occured";
                timerPart.targetTurns = 3000L; // a few moments later
                The.Player.AddPart(timerPart);

                // Attach Outcome Part to the player
                var outcomePart = new Brothers_Tortoises_PoacherOutcome();    
                The.Player.AddPart(outcomePart);                                    
            }
        }
        
        public override GameObject GetInfluencer() =>
          GameObject.FindByBlueprint("Brothers_Tortoises_Cracks-the-Saltback-Shell");
    }
}
