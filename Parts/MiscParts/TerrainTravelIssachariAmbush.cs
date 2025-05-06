using System;
using XRL.Rules;
using XRL.UI;
using XRL.World.Effects;
using XRL.World.ZoneBuilders;
using XRL.World.ZoneParts;
using System.Collections.Generic;

#nullable disable
namespace XRL.World.Parts
{
    [Serializable]
    public class TerrainTravelIssachariAmbush : IPart
    {
        public override bool SameAs(IPart p) => true;

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            //use this to check terrain change in the overworld
            Registrar.Register("CheckLostChance");
            base.Register(Object, Registrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "CheckLostChance")
            {
                // Dont want this to happen if IDKFA cheat is on
                if (The.Core.IDKFA)
                    return false;

                // 1 in 4 chance
                if (Stat.Random(1, 4) == 1)
                {

                    // Make sure the player has pissed off the poacher
                    if (The.Game.GetBooleanGameState("HatedByPoacher"))
                    {
                        // Check that the player has not yet been ambushed
                        if (!The.Game.HasStringGameState("HaveBeenAmbushedByPoacher"))
                        {
                            Cell currentCell = The.Player.CurrentCell;
                            string zoneWorld = currentCell.ParentZone.GetZoneWorld();
                            int x = currentCell.X;
                            int y = currentCell.Y;
                            int ZoneX = Stat.Random(0, 2);
                            int ZoneY = Stat.Random(0, 2);
                            int ZoneZ = 10;
                            string str = ZoneID.Assemble(zoneWorld, x, y, ZoneX, ZoneY, ZoneZ);

                            // apply effect
                            if (The.Player.ApplyEffect((Effect)new Lost(InitialZone: str, World: zoneWorld)))
                            {
                                // set the already ambushed flag
                                The.Game.SetStringGameState("HaveBeenAmbushedByPoacher", str);
                                //regular lost logic
                                Zone zone = The.ZoneManager.SetActiveZone(The.ZoneManager.GetZone(str));
                                zone.CheckWeather();
                                The.Player.SystemMoveTo(zone.GetPullDownLocation(The.Player));
                                int num = (int)Popup.ShowBlock("Silhouettes sporting the Issachari red-and-white pop out of the salt where they laid in ambush! Their leader wields an impressive weapon bearing a familiar mark.");
                                The.ZoneManager.ProcessGoToPartyLeader();
                                The.Player.FireEvent(Event.New("AfterLost", "FromCell", (object)currentCell));

                                // Add Issachari ambushers
                                // Get the current cell after the player has been lost
                                Cell currentCellPostLost = The.Player.CurrentCell;

                                // This is where we set the Population to spawn
                                List<PopulationResult> party = PopulationManager.Generate("Brothers_Tortoises_Issachari Ambushers");

                                // create a list to track used spawn cells
                                List<Cell> usedSpawnCells = new List<Cell>();

                                foreach (PopulationResult result in party)
                                {
                                    for (int i = 0; i < result.Number; i++)
                                    {
                                        // Try to find a free cell
                                        Cell spawnCellAmbushers = null;
                                        for (int attempt = 0; attempt < 20; attempt++) // Try up to 20 times
                                        {
                                            Cell candidate = currentCellPostLost.GetRandomLocalAdjacentCellAtRadius(Stat.Random(4, 8));
                                            if (candidate != null && !usedSpawnCells.Contains(candidate))
                                            {
                                                spawnCellAmbushers = candidate;
                                                break;
                                            }
                                        }

                                        if (spawnCellAmbushers != null)
                                        {
                                            usedSpawnCells.Add(spawnCellAmbushers); // Mark this cell as used

                                            GameObject spawned = GameObject.Create(result.Blueprint);
                                            spawnCellAmbushers.AddObject(spawned);
                                            // Make Player Hater
                                            spawned.Brain.Allegiance.Add("Playerhater", 500);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return base.FireEvent(E);
        }
    }
}
