using XRL;
using XRL.World;
using XRL.World.WorldBuilders;
using XRL.World.ZoneBuilders;
using XRL.World.Parts;
using System.Collections.Generic;

namespace Tortoises.Brothers
{
    [JoppaWorldBuilderExtension]
    public class Brothers_Tortoises_SaltbackGraveyardWorldBuilderExtension : IJoppaWorldBuilderExtension
    {
        public override void OnAfterBuild(JoppaWorldBuilder builder)
        {
            // Pick a location to place Saltback Graveyard
            var location = builder.popMutableLocationOfTerrain("Saltdunes", centerOnly: false);
            var zoneID = builder.ZoneIDFromXY("JoppaWorld", location.X, location.Y);

            // Add a dynamic secret for Saltback Graveyard
            builder.AddSecret(
                zoneID,
                "the Saltback Graveyard",
                new string[] { "settlement", "tortoises","saltbackgraveyard" },
                "Settlements",
                "$saltbackgraveyard"
            );

            // Set terrain to Saltback Graveyard
            Cell cell = The.ZoneManager.GetZone("JoppaWorld").GetCell(location.X / 3, location.Y / 3);
            cell.GetFirstObjectWithPart("TerrainTravel")?.AddPart<Brothers_Tortoises_SaltbackGraveyardTerrain>(new Brothers_Tortoises_SaltbackGraveyardTerrain());

            // Set zone builders and properties
            var zoneManager = The.ZoneManager;

            zoneManager.SetZoneProperty(zoneID, "NoBiomes", "Yes");
            zoneManager.SetZoneProperty(zoneID, "SkipTerrainBuilders", true);
            zoneManager.AddZoneBuilder(zoneID, 6000, "SaltDunes");
            zoneManager.AddZonePostBuilder(zoneID, "MapBuilder", "FileName", "Brothers_SaltbackGraveyard.rpm");
            zoneManager.AddZonePostBuilder(zoneID, "Music", "Track", "Music/tortoises/Baud.mp3");
            zoneManager.AddZonePostBuilder(zoneID, "AddWidgetBuilder", "Blueprint", "Brothers_Tortoises_SaltbackGraveyardSurface");
            zoneManager.AddZonePostBuilder(zoneID, "IsCheckpoint", "Key", zoneID);

            zoneManager.SetZoneName(zoneID, "Saltback Graveyard", Proper: true);
            zoneManager.SetZoneIncludeStratumInZoneDisplay(zoneID, false);

            var locationInfo = new GeneratedLocationInfo
            {
                name = "Saltback Graveyard",
                targetZone = zoneID,
                zoneLocation = location,
                secretID = "$saltbackgraveyard"
            };

            builder.worldInfo.villages.Add(locationInfo);
            builder.mutableMap.SetMutable(location, 0);

            // Save the zone ID in game state
            builder.game.SetStringGameState("SaltbackGraveyardZoneID", zoneID);
        }
    }
}
