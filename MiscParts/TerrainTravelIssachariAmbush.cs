using System;
using XRL.Rules;
using XRL.UI;
using XRL.World.Effects;
using XRL.World.ZoneBuilders;

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
                // Make sure the player has pissed off the poacher
                if (The.Game.HasQuest("Return to Qoruda"))
                {
                    // Check that the player has not yet been ambushed
                    if (The.Game.HasStringGameState("HaveBeenAmbushedByPoacher"))
                        return false;


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
                        int num = (int)Popup.ShowBlock("You are ambushed by Issachari hunters!");
                        The.ZoneManager.ProcessGoToPartyLeader();
                        The.Player.FireEvent(Event.New("AfterLost", "FromCell", (object)currentCell));
                        return false;
                    }
                }
            }
            return base.FireEvent(E);
        }
    }
}