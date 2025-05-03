using Qud.API;
using System;
using XRL.World;

namespace XRL.World.Conversations.Parts
{
    public class RevealGraveyardLocation : IConversationPart
    {
        public override bool WantEvent(int ID, int Propagation)
        {
            return base.WantEvent(ID, Propagation) ||
                   ID == EnteredElementEvent.ID ||
                   ID == GetChoiceTagEvent.ID;
        }

        public override bool HandleEvent(EnteredElementEvent E)
        {
            ZoneManager.instance.GetZone("JoppaWorld").BroadcastEvent("SaltbackGraveyardReveal");
            The.Game.SetStringGameState("GraveyardKnown", "1");
            return base.HandleEvent(E);
        }
    }
}