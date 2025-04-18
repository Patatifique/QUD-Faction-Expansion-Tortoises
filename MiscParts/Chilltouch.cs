using System;
using XRL.World.Parts;

namespace XRL.World.Parts
{
    [Serializable]
    public class Chilltouch : IPart
    {
        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == PooledEvent<PhysicalContactEvent>.ID;
        }

        public override bool HandleEvent(PhysicalContactEvent E)
        {
            if (E.Object == this.ParentObject && E.Actor != E.Object && GameObject.Validate(E.Actor))
            {
                // Reduce the temperature of the actor by 100 units
                E.Actor.TemperatureChange(-100, null, Min: null, Max: null);
            }

            return base.HandleEvent(E);
        }

        public override bool AllowStaticRegistration() => true;
    }
}
