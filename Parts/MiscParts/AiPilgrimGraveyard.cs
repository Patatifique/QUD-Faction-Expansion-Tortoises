
using Qud.API;
using System;
using System.Collections.Generic;
using System.Linq;
using XRL.World.AI;
using XRL.World.AI.GoalHandlers;
using XRL.World.Effects;

#nullable disable
namespace XRL.World.Parts
{
    [Serializable]
    public class AiPilgrimGraveyard : AIBehaviorPart
    {
        public bool FoundTarget;
        public int GraveyardWx = 5;
        public int GraveyardWy = 2;
        public int GraveyardXx = 1;
        public int GraveyardYx = 1;
        public int GraveyardZx = 10;
        public string GraveyardZoneID = The.Game.GetStringGameState("SaltbackGraveyardZoneID");
        public string GraveyardEntranceZoneID = The.Game.GetStringGameState("SaltbackGraveyardZoneID");
        public string TargetObject = "FakeStillWell";
        public string MapNoteAttributes;
        public int Chance = 100;
        public bool Ignore;

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == PooledEvent<AIBoredEvent>.ID || ID == PooledEvent<GetItemElementsEvent>.ID;
        }

        public override bool HandleEvent(AIBoredEvent E)
        {
            return !this.CheckStartPilgrimage() && base.HandleEvent(E);
        }

        public override bool HandleEvent(GetItemElementsEvent E)
        {
            if (E.IsRelevantCreature(this.ParentObject))
                E.Add("travel", 1);
            return base.HandleEvent(E);
        }

        public override void Initialize()
        {
            if (!this.Chance.in100())
                this.Ignore = true;
            base.Initialize();
        }

        public bool CheckStartPilgrimage()
        {
            if (this.Ignore || !this.ParentObject.FireEvent("CanAIDoIndependentBehavior") || this.ParentObject.PartyLeader != null || this.ParentObject.HasTagOrProperty("ExcludeFromDynamicEncounters") || this.FoundTarget || this.ParentObject.HasIntProperty("LairOwner") || this.ParentObject.HasEffect<Lost>() || this.ParentObject.HasEffect<XRL.World.Effects.Confused>() && !this.ParentObject.HasEffect<FuriouslyConfused>())
                return false;
            if (!string.IsNullOrEmpty(this.MapNoteAttributes))
            {
                if (this.MapNoteAttributes == "invalid-tag" || this.ParentObject.CurrentZone == null)
                    return false;
                IEnumerable<JournalMapNote> withAllAttributes = (IEnumerable<JournalMapNote>)JournalAPI.GetMapNotesWithAllAttributes(this.MapNoteAttributes);
                this.MapNoteAttributes = "invalid-tag";
                if (withAllAttributes.Count<JournalMapNote>() == 0)
                    return false;
                JournalMapNote randomElement = withAllAttributes.ToList<JournalMapNote>().GetRandomElement<JournalMapNote>();
                this.GraveyardWx = randomElement.ParasangX;
                this.GraveyardWy = randomElement.ParasangY;
                this.GraveyardXx = randomElement.ZoneX;
                this.GraveyardYx = randomElement.ZoneY;
                this.GraveyardZx = randomElement.ZoneZ;
                this.GraveyardZoneID = randomElement.ZoneID;
                this.GraveyardEntranceZoneID = randomElement.ZoneID;
            }
            this.ParentObject.Brain.PushGoal((GoalHandler)new GoOnAPilgrimage(this.GraveyardWx, this.GraveyardWy, this.GraveyardXx, this.GraveyardYx, this.GraveyardZx, this.TargetObject, this.GraveyardZoneID, this.GraveyardEntranceZoneID));
            return true;
        }
    }
}