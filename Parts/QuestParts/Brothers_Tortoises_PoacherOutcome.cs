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

            // Destroy everything that should be destroyed
            DestroyOnEnding();

            // Replace the stele
            ReplaceObject("Brothers_Tortoises_BigStele", "Brothers_Tortoises_RuinedStele");

            // Add the camp
            this.ParentObject.CurrentZone.GetCell(12, 4).AddObject("Brothers_Tortoises_IssachariCamp");
            
            // Add a few random issachari to the camp 
            PopulateRandomly(1, 3, "Brothers_Tortoises_IssachariRaiderChill");
            PopulateRandomly(1, 3, "Brothers_Tortoises_IssachariRiflerChill");

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
                    if (gameObject.Blueprint != "Brothers_Tortoises_Walking Dune" && gameObject.Blueprint != "Brothers_Tortoises_Warden" )
                        continue;

                if (gameObject.HasTag("Brothers_Tortoises_DestroyedOnEnding"))
                {
                    gameObject.Obliterate();
                }
            }
        }

        public void ReplaceObject(string oldObjectBlueprint, string newObjectBlueprint)
        {

            foreach (GameObject gameObject in this.ParentObject.CurrentZone.GetObjects(o => o.Blueprint == oldObjectBlueprint))
            {
                var cell = gameObject.Physics?.CurrentCell;
                if (cell != null)
                {
                    cell.RemoveObject(gameObject);
                    cell.AddObject(newObjectBlueprint);
                }
            }
        }

        public void PopulateRandomly(int min, int max, string blueprint)
        {
                int num1 = 0;
                for (int index = Stat.Random(min, max); num1 < index; ++num1)
                    ((IEnumerable<Cell>) this.ParentObject.CurrentZone.GetEmptyCellsShuffled()).First<Cell>().AddObject(blueprint);
        }

    }
}