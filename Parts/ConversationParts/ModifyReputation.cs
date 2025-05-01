// This is a conversation part that modifies the player's reputation with a specified faction when used
// It also displays the reputation change in the dialogue choice as a color-coded tag
// Usage in XML: <part Name="ModifyReputation" Faction="Tortoises" Value="100" Shown="True" />

// If Faction is not set, it defaults to the speaker's primary faction. If Shown is false or not set, the tag won't be rendered

using XRL.World.Conversations.Parts;

namespace XRL.World.Conversations.Parts
{
    public class ModifyReputation : IConversationPart
    {
        public string Faction;
        public int Value = int.MaxValue;
        public bool Shown = false;

        public override bool WantEvent(int ID, int Propagation)
        {
            return base.WantEvent(ID, Propagation)
                || ID == EnterElementEvent.ID
                || ID == GetChoiceTagEvent.ID;
        }

        public override bool HandleEvent(EnterElementEvent E)
        {
            The.Game.PlayerReputation.Modify(this.Faction ?? The.Speaker.GetPrimaryFaction(), this.Value);
            return true;
        }

        public override bool HandleEvent(GetChoiceTagEvent E)
        {
            //skip this if not shown
            if (!this.Shown)
                return false;
            //get speaker faction if not set
            XRL.World.Faction faction = Factions.Get(this.Faction ?? The.Speaker.GetPrimaryFaction());

            //get the color based on the value
            char color = this.Value < 0 ? 'r' : 'C';

            //format the value with a + if positive
            var formattedValue = Value >= 0 ? $"+{Value}" : Value.ToString();

            E.Tag = $"{{{{{color}|[{formattedValue} reputation with {faction.GetFormattedName()}]}}}}";
            return false;
        }
    }
}
