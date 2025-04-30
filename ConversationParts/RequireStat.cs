// This is a new conversation part for checking player stats in conversations.
//Usage in xml is as follows: <part Name="RequireStat" Stat="stat checked" Value="value wanted" />

namespace XRL.World.Conversations.Parts
{
    public class RequireStat : IConversationPart
    {
        public string Stat;
        public int Value = int.MaxValue;
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
            if (!string.IsNullOrEmpty(Stat))
            {
                GameObject subject = The.Player;
                Fulfilled = subject.Stat(Stat) >= Value;
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EnterElementEvent E)
        {
            return Fulfilled;
        }

        public override bool HandleEvent(GetChoiceTagEvent E)
        {
            char ch = Fulfilled ? 'C' : 'r';
            E.Tag = $"{{{{{ch}|[{Stat} ≥ {Value}]}}}}";
            return false;
        }

        public override bool HandleEvent(ColorTextEvent E)
        {
            if (Fulfilled)
                return base.HandleEvent(E);
            E.Color = "K";
            return false;
        }
    }
}
