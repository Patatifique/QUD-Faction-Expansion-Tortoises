using Qud.API;
using System;

#nullable disable
namespace XRL.World.Parts
{
    [Serializable]
    public class SaltbackGraveyardTerrain : IPart
    {
        public string secretId = "$saltbackgraveyard";
        public bool revealed;

        public override bool SameAs(IPart p) => false;

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Object.SetIntProperty("ForceMutableSave", 1);
            Registrar.Register("SaltbackGraveyardReveal");
            base.Register(Object, Registrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "SaltbackGraveyardReveal")
            {
                this.ParentObject.Render.Tile = "Terrain/tortoises/map_graveyard.png";
                this.ParentObject.Render.ColorString = "&Y";
                this.ParentObject.Render.DisplayName = "Saltback Graveyard";
                this.ParentObject.Render.DetailColor = "W";
                this.ParentObject.Render.RenderString = "#";
                this.ParentObject.HasProperName = true;
                this.ParentObject.GetPart<Description>().Short = "Nestled between windshaped dunes, a concealed valley holds an ocean of bones, a testament to the fleetingness of life in Qud.";
                this.ParentObject.GetPart<TerrainTravel>()?.ClearEncounters();
                this.ParentObject.SetStringProperty("OverlayColor", "&W");
                if (this.secretId != null)
                {
                    JournalMapNote mapNote = JournalAPI.GetMapNote(this.secretId);
                    if (mapNote != null && !mapNote.Revealed)
                        mapNote.Reveal((string)null, false);
                }
            }
            return base.FireEvent(E);
        }
    }
}
