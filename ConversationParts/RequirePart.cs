// This is a new conversation part for checking player stats in conversations and have the option show up as grayed out if player doesn't have the part.
// Usage in xml is as follows: <part Name="RequirePart" Part="part checked (from class name)" Render="display of requirement ingame" />
// If Render is not set, it will use the name of the part

using XRL.World.Conversations.Parts;

namespace XRL.World.Conversations.Parts
{
    public class RequirePart : IConversationPart
    {
        public string Part;
        public string Render;
        public bool Fulfilled;

        public override bool WantEvent(int ID, int Propagation)
        {
            return base.WantEvent(ID, Propagation)
                || ID == PrepareTextEvent.ID
                || ID == EnterElementEvent.ID
                || ID == GetChoiceTagEvent.ID
                || ID == ColorTextEvent.ID;
        }

        public override bool HandleEvent(PrepareTextEvent E)
        {
            if (!string.IsNullOrEmpty(Part))
            {
                GameObject subject = The.Player;
                this.Fulfilled = subject.HasPart(Part);
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EnterElementEvent E)
        {
            return this.Fulfilled;
        }

        public override bool HandleEvent(GetChoiceTagEvent E)
        {
            char ch = this.Fulfilled ? 'C' : 'r';
            E.Tag = $"{{{{{ch}|[{Render ?? Part}]}}}}";
            return false;
        }

        public override bool HandleEvent(ColorTextEvent E)
        {
            if (this.Fulfilled)
                return base.HandleEvent(E);
            E.Color = "K";
            return false;
        }
    }
}
