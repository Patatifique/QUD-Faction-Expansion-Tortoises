// This is a conversation part that modifies the player's reputation with one or two specified faction when used
// It also displays the reputation change in the dialogue choice as a color-coded tag
// Usage in XML: <part Name="ModifyReputation" Faction="Tortoises" Value="100" Faction2="Issachari" Value2="-100" Shown="True" />

// If Faction is not set, it defaults to the speaker's primary faction.
// If Shown is false or not set, the tag won't be rendered
// If Faction2 and Value2 are set, both reputations will be modified and shown in one combined tag


using XRL.World.Conversations.Parts;

namespace XRL.World.Conversations.Parts
{
    public class ModifyReputation : IConversationPart
    {
        public string Faction;
        public int Value = int.MaxValue;
        public string Faction2;
        public int Value2 = int.MaxValue;
        public bool Shown = false;

        public override bool WantEvent(int ID, int Propagation)
        {
            return base.WantEvent(ID, Propagation)
                || ID == EnterElementEvent.ID
                || ID == GetChoiceTagEvent.ID;
        }

        public override bool HandleEvent(EnterElementEvent E)
        {
            //modify the reputation of the specified faction or the speaker's primary faction if not set
            The.Game.PlayerReputation.Modify(this.Faction ?? The.Speaker.GetPrimaryFaction(), this.Value);            
            //if a second faction is specified, modify its reputation as well
            if (this.Faction2 != null && this.Value2 != int.MaxValue)
            {
                The.Game.PlayerReputation.Modify(this.Faction2, this.Value2);
            }
            return true;
        }

        public override bool HandleEvent(GetChoiceTagEvent E)
        {
            // skip this if not shown
            if (!this.Shown)
                return false;
            
            //get speaker faction if not set, assign faction and value to variables
            var f1 = Factions.Get(this.Faction ?? The.Speaker.GetPrimaryFaction());
            var v1 = this.Value;
            var formatted1 = v1 >= 0 ? $"+{v1}" : v1.ToString();
            var result = $"{formatted1} reputation with {f1.GetFormattedName()}";

            // add second faction info if available
            if (this.Faction2 != null && this.Value2 != int.MaxValue)
            {
                var f2 = Factions.Get(this.Faction2);
                var v2 = this.Value2;
                var formatted2 = v2 >= 0 ? $"+{v2}" : v2.ToString();
                result += $", {formatted2} reputation with {f2.GetFormattedName()}";
                
                // set red color only if both are negative
                char color = (v1 < 0 && v2 < 0) ? 'r' : 'C';
                E.Tag = $"{{{{{color}|[{result}]}}}}";
            }
            else
            {
                // single faction color logic
                char color = v1 < 0 ? 'r' : 'C';
                E.Tag = $"{{{{{color}|[{result}]}}}}";
            }

            return false;
        }
    }
}
