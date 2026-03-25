using System;
using XRL.World;
using XRL.UI;
using XRL.Rules;
using System.Collections.Generic;
using System.Linq;

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
                string zoneID = The.Game.GetStringGameState("SaltbackGraveyardZoneID");
                 if (E.Zone.ZoneID == zoneID)
                 {
                    this.ApplyOutcome();
                 }       
            }
            return base.HandleEvent(E);
        }


        private void ApplyOutcome()
        {
            // Debug message
            Popup.Show("debug: applying poacher outcome");

            // Change Terrain Data
            ReplaceTerrainData(
                "r",
                "new sad description");

            // Destroy bones
            DestroyOnEnding();

            // Remove this part after applying the outcome
            this.ParentObject.RemovePart(this);
        }
        
        public void ReplaceTerrainData(string newDetail, string newDescription)
        {
            Cell cell = The.ZoneManager.GetZone("JoppaWorld").GetCell(this.ParentObject.CurrentZone.wX, this.ParentObject.CurrentZone.wY);
            var terrain = cell.GetFirstObjectWithPart("TerrainTravel");
            if (terrain != null)
            {
                terrain.Render.DetailColor = newDetail;
                terrain.GetPart<Description>().Short = newDescription;
            }
        }

        public void DestroyOnEnding()
        {
            foreach (GameObject gameObject in this.ParentObject.CurrentZone.GetObjects(o =>
                o.HasTag("Brothers_Tortoises_DestroyedOnEnding")))
            {
                if (gameObject.HasPart<Brain>())
                    continue;

                if (gameObject.HasTag("Brothers_Tortoises_DestroyedOnEnding"))
                {
                    gameObject.Obliterate();
                }
            }
        }


    }
}