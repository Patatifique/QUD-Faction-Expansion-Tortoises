// Modified but basically copy pasted from CryoGas just to get a different log message....

using System;
using System.Collections.Generic;

#nullable disable
namespace XRL.World.Parts
{
    [Serializable]
    public class GasFreezing : IGasBehavior
    {
        public string GasType = "Freezing";

        public override bool SameAs(IPart p)
        {
            return !((p as GasFreezing).GasType != this.GasType) && base.SameAs(p);
        }

        public override bool WantEvent(int ID, int cascade)
        {
            return base.WantEvent(ID, cascade) || ID == SingletonEvent<EndTurnEvent>.ID || ID == GetAdjacentNavigationWeightEvent.ID || ID == GetNavigationWeightEvent.ID || ID == ObjectEnteredCellEvent.ID;
        }

        public override bool HandleEvent(GetNavigationWeightEvent E)
        {
            if (!E.IgnoreGases && E.PhaseMatches(this.ParentObject))
            {
                if (E.Smart)
                {
                    E.Uncacheable = true;
                    if (CheckGasCanAffectEvent.Check(E.Actor, this.ParentObject) && (E.Actor == null || E.Actor.PhaseMatches(this.ParentObject)))
                    {
                        int num1 = this.GasDensityStepped() / 2 + 3;
                        if (E.Actor != null)
                        {
                            int num2 = E.Actor.Stat("ColdResistance");
                            if (num2 != 0)
                                num1 = Math.Max(num1 * (100 - num2) / 100, 0);
                        }
                        if (num1 > 0)
                            E.MinWeight(num1, 53);
                    }
                }
                else
                    E.MinWeight(3);
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(GetAdjacentNavigationWeightEvent E)
        {
            if (!E.IgnoreGases && E.PhaseMatches(this.ParentObject))
            {
                if (E.Smart)
                {
                    E.Uncacheable = true;
                    if (CheckGasCanAffectEvent.Check(E.Actor, this.ParentObject) && (E.Actor == null || E.Actor.PhaseMatches(this.ParentObject)))
                    {
                        int num1 = this.GasDensityStepped() / 10 + 1;
                        if (E.Actor != null)
                        {
                            int num2 = E.Actor.Stat("ColdResistance");
                            if (num2 != 0)
                                num1 = Math.Max(num1 * (100 - num2) / 100, 0);
                        }
                        if (num1 > 0)
                            E.MinWeight(num1, 11);
                    }
                }
                else
                    E.MinWeight(1);
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(EndTurnEvent E)
        {
            Cell currentCell = this.ParentObject.CurrentCell;
            if (currentCell != null)
            {
                List<GameObject> gameObjectList = Event.NewGameObjectList((IEnumerable<GameObject>)currentCell.Objects);
                int index = 0;
                for (int count = gameObjectList.Count; index < count; ++index)
                {
                    GameObject GO = gameObjectList[index];
                    if (GO != this.ParentObject && !GO.IsScenery)
                        this.ApplyFreezing(GO);
                }
            }
            return base.HandleEvent(E);
        }

        public override bool HandleEvent(ObjectEnteredCellEvent E)
        {
            if (E.Object != this.ParentObject && E.Type != "Thrown")
                this.ApplyFreezing(E.Object);
            return base.HandleEvent(E);
        }

        public override void Register(GameObject Object, IEventRegistrar Registrar)
        {
            Registrar.Register("DensityChange");
            base.Register(Object, Registrar);
        }

        public void ApplyFreezing(GameObject GO)
        {
            Gas part = this.ParentObject.GetPart<Gas>();
            if (!CheckGasCanAffectEvent.Check(GO, this.ParentObject, part) || !GO.PhaseMatches(this.ParentObject))
                return;
            Event.PinCurrentPool();
            int num = (int)Math.Ceiling(2.5 * (double)part.Density);
            if (GO.Physics.Temperature > -num)
                GO.TemperatureChange(-num, this.ParentObject, Phase: this.ParentObject.GetPhase());
            if (GO.IsPlayer() || !GO.IsFrozen())
                GO.TakeDamage(1, "from the {{icy|freezing gas}}.", "Cold", Owner: part.Creator, Source: this.ParentObject, Environmental: true);
            Event.ResetToPin();
        }

        public override bool FireEvent(Event E)
        {
            if (E.ID == "DensityChange" && this.StepValue(E.GetIntParameter("OldValue")) != this.StepValue(E.GetIntParameter("NewValue")))
                this.FlushNavigationCaches();
            return base.FireEvent(E);
        }
    }
}
