using Qud.API;
using System;

#nullable disable
namespace XRL.World.Parts
{
    [Serializable]
    public class SaltbackGraveyardSurface : IPart
    {
        public string RegionName = "Saltback Graveyard";
        public string RevealString;
        public string RevealSecret = "$saltbackgraveyard";
        public string RevealKey = "SaltbackGraveryard_LocationKnown";

        public override bool SameAs(IPart p) => false;

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register("EnteredCell");
            base.Register(Object, Registrar);
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "EnteredCell" && !The.Game.HasIntGameState(this.RevealKey))
            {
                The.Game.SetIntGameState(this.RevealKey, 1);
                ZoneManager.instance.GetZone("JoppaWorld").BroadcastEvent("SaltbackGraveyardReveal");
                The.Game.SetStringGameState("GraveyardKnown", str);
                JournalMapNote mapNote = JournalAPI.GetMapNote(this.RevealSecret);
                if (mapNote != null && !mapNote.Revealed)
                {
                    JournalAPI.AddAccomplishment("You discovered the hidden Saltback Graveyard.", "<spice.commonPhrases.intrepid.!random.capitalize> =name= discovered the Saltback Graveyard, once thought lost to the sands of time.", $"<spice.commonPhrases.allThroughout.!random.capitalize> =year=, =name= followed migrating saltbacks through Moghra'yi. {The.Player.GetPronounProvider().CapitalizedSubjective} became known as the Saltbacked Pilgrim.", muralCategory: MuralCategory.VisitsLocation);
                    JournalAPI.RevealMapNote(mapNote);
                }
            }
            return base.FireEvent(E);
        }
    }
}
