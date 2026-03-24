using System;
using XRL.World;
using XRL.UI;

namespace XRL.World.Parts
{
    [Serializable]
    public class Brothers_Tortoises_PoacherOutcome : IPart
    {  
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade)
                || ID == ZoneActivatedEvent.ID;
        }


        public override bool HandleEvent(ZoneActivatedEvent E)
        {
            if (The.Game.GetBooleanGameState("Brothers_Tortoises_PoacherEnding_Occured"))
            {       
                this.ApplyOutcome();
            }
            return base.HandleEvent(E);
        }

        private void ApplyOutcome()
        {
            Popup.Show("debug");
        }
    }
}